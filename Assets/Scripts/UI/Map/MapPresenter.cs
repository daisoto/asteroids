using System;
using UniRx;
using System.Collections.Generic;
using System.Linq;
using Data;
using Gameplay;
using UnityEngine;
using Zenject;

namespace UI
{
public class MapPresenter: Presenter<MapView>, IInitializable, IDisposable
{
    private readonly LevelsSettings _levelsSettings;
    private readonly LevelsController _levelsController;
    private readonly SignalBus _signalBus;
    
    private readonly List<PlanetModel> _models;
    private readonly DisposablesContainer _disposablesContainer;
    
    private Action _onBack;
    private Action _onPlanetChose;

    private Color _finishedColor => _levelsSettings.FinishedColor; 
    private Color _openedColor => _levelsSettings.OpenedColor; 
    private Color _closedColor => _levelsSettings.ClosedColor; 
    
    public MapPresenter(MapView view, 
        LevelsController levelsController, 
        LevelsSettings levelsSettings, SignalBus signalBus) : base(view)
    {
        _levelsController = levelsController;
        _levelsSettings = levelsSettings;
        _signalBus = signalBus;

        _disposablesContainer = new DisposablesContainer();
        _models = new List<PlanetModel>();
    }

    public void Initialize()
    {
        UpdatePlanetModels();
        BindModels();
        
        _view
            .SetOnBack(Back);
        
        _signalBus.Subscribe<LevelStartedSignal>(Close);
        _signalBus.Subscribe<SetSpaceshipDataSignal>(UpdatePlanetModels);
        _signalBus.Subscribe<LevelFinishedSignal>(SetFinished);
    }
    
    public void Dispose() 
    {
        _signalBus.Unsubscribe<LevelStartedSignal>(Close);
        _signalBus.Unsubscribe<SetSpaceshipDataSignal>(UpdatePlanetModels);
        _signalBus.Unsubscribe<LevelFinishedSignal>(SetFinished);
        _disposablesContainer.Dispose();
    }

    public MapPresenter SetOnBack(Action onBack)
    {
        _onBack = onBack;
        
        return this;
    }

    public MapPresenter SetOnPlanetChose(Action onPlanetChose)
    {
        _onPlanetChose = onPlanetChose;
        
        return this;
    }
    
    private void Back()
    {
        _onBack?.Invoke();
        Close();
    }
    
    private void UpdatePlanetModels()
    {
        var maxLevel = _levelsSettings.MaxLevel;
        var levelsDataDict = GetLevelsDataDict();
        
        for (int level = 1; level <= maxLevel; level++)
        {
            var isFirst = level == 1;
            
            var data = levelsDataDict.ContainsKey(level) ? 
                levelsDataDict[level] : null;
            
            var prevData = levelsDataDict.ContainsKey(level - 1) ? 
                levelsDataDict[level - 1] : null;
            
            var isFinished = data is {IsFinished: true};
            var isPrevFinished = prevData is {IsFinished: true};
            var isAvailable = isFirst || isPrevFinished;
            
            if (_models.Count >= level)
            {
                _models[level - 1].IsAvailable.Value = isAvailable;
                _models[level - 1].IsFinished.Value = isFinished;
            }
            else
                _models.Add(
                    new PlanetModel(isAvailable, isFinished));
            
        }
    }
    
    private void BindModels()
    {
        for (int i = 0; i < _models.Count; i++)
        {
            var level = i + 1;
            var model  = _models[i];
            
            _view.SetOnClick(level, Select);
            
            void Select()
            {
                if (model.IsAvailable.Value)
                    StartLevel(level);
            }
            
            _disposablesContainer.Add(model.IsAvailable
                .Subscribe(isAvailable =>
                {
                    _view.PaintPlanet(level,
                        isAvailable ? _openedColor : _closedColor);
                }));
            
            _disposablesContainer.Add(model.IsFinished
                .Subscribe(isFinished =>
                {
                    if (isFinished)
                        _view.PaintPlanet(level, _finishedColor);
                }));
        }
    }
    
    private Dictionary<int, LevelData> GetLevelsDataDict()
    {
        return _levelsController
            .SavedLevelsData
            .ToDictionary(ld => ld.Level, ld => ld);
    }
    
    private void StartLevel(int level)
    {
        _onPlanetChose?.Invoke();
        _levelsController.StartLevel(level);
    }

    private void SetFinished(LevelFinishedSignal signal)
    {
        var level = signal.Level;
        
        _models[level - 1].IsFinished.Value = true;
        
        if (_models.Count > level)
            _models[level].IsAvailable.Value = true;
    }
        
}
}