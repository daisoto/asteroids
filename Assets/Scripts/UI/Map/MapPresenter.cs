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
    private readonly LevelsController _levelsController;
    private readonly ILevelsViewDataProvider _levelsViewDataProvider;
    
    private readonly List<PlanetModel> _models;
    private readonly DisposablesContainer _disposablesContainer;
    
    private Action _onBack;

    private Color _finishedColor => _levelsViewDataProvider.FinishedColor; 
    private Color _openedColor => _levelsViewDataProvider.OpenedColor; 
    private Color _closedColor => _levelsViewDataProvider.ClosedColor; 
    
    public MapPresenter(MapView view, 
        LevelsController levelsController, 
        ILevelsViewDataProvider levelsViewDataProvider) : base(view)
    {
        _levelsController = levelsController;
        _levelsViewDataProvider = levelsViewDataProvider;
        
        _disposablesContainer = new DisposablesContainer();
        _models = new List<PlanetModel>();
    }

    public void Initialize()
    {
        CreatePlanetModels();
        BindModels();
        
        _view
            .SetOnBack(Back);
        
        _levelsController
            .SetOnLevelStart(Close)
            .SetOnLevelFinished(Show);
    }
    
    public void Dispose() => _disposablesContainer.Dispose();

    public MapPresenter SetOnBack(Action onBack)
    {
        _onBack = onBack;
        
        return this;
    }
    
    private void Back()
    {
        _onBack?.Invoke();
        Close();
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
    
    private void CreatePlanetModels()
    {
        var maxLevel = _levelsViewDataProvider.MaxLevel;
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
            
            var planetModel = new PlanetModel(isAvailable, isFinished);
            
            _models.Add(planetModel);
        }
    }
    
    private Dictionary<int, LevelData> GetLevelsDataDict()
    {
        return _levelsController
            .LevelsData
            .ToDictionary(ld => ld.Level, ld => ld);
    }
    
    private void StartLevel(int level) => 
        _levelsController.StartLevel(level, SetFinished);

    private void SetFinished(int level) =>
        _models[level].IsFinished.Value = true;
}
}