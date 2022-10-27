using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Data;

namespace Gameplay
{
public class LevelController: IInitializable, IDisposable
{
    private readonly AsteroidsController _asteroidsController;
    private readonly IWorldPointProvider _worldPointProvider;
    private readonly SignalBus _signalBus;
    
    private readonly List<AsteroidModel> _sleepingAsteroidModels;
    private readonly List<AsteroidModel> _activeAsteroidModels;
    
    private int _asteroidsNum; 
    private int _currentLevel;
    private float _delay;
    private bool _isPlaying;
    private Action<int> _onLevelFinished;

    public LevelController(AsteroidsController asteroidsController, 
        IWorldPointProvider worldPointProvider, SignalBus signalBus)
    {
        _asteroidsController = asteroidsController;
        _worldPointProvider = worldPointProvider;
        _signalBus = signalBus;
        
        _sleepingAsteroidModels = new List<AsteroidModel>();
        _activeAsteroidModels = new List<AsteroidModel>();
    }

    public void Initialize()
    {
        _signalBus.Subscribe<AsteroidCollapseSignal>(ProcessCollapse);
        _signalBus.Subscribe<SpaceshipDestroyedSignal>(StopPlaying);
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<AsteroidCollapseSignal>(ProcessCollapse);
        _signalBus.Unsubscribe<SpaceshipDestroyedSignal>(StopPlaying);
    }
    
    public LevelController SetOnLevelFinished(Action<int> onLevelFinished)
    {
        _onLevelFinished = onLevelFinished;
        
        return this;
    }
    
    public LevelController SetDelay(float delay)
    {
        _delay = delay;
        
        return this;
    }
    
    public void StartLevel(LevelData levelData)
    {
        _isPlaying = true;
        DeactivateRemaining();
        _currentLevel = levelData.Level;
        
        var sizes = EnumUtils.GetValues<AsteroidSize>();
        foreach (var size in sizes)
        {
            var num = levelData.AsteroidsNum[size];
            _asteroidsNum += num;
            
            for (int i = 0; i < num; i++)
            {
                var asteroid = _asteroidsController
                    .CreateAsteroid(size);
                _sleepingAsteroidModels.Add(asteroid);
            }
        }
        
        _signalBus.Fire(new ResumeGameSignal());
        LevelSequence().Forget();
    }
    
    private void StopPlaying() => _isPlaying = false;
    
    private void DeactivateRemaining()
    {
        _activeAsteroidModels.ForEach(m => m.Deactivate()); 
        _sleepingAsteroidModels.ForEach(m => m.Deactivate()); 
        
        _activeAsteroidModels.Clear();
        _sleepingAsteroidModels.Clear();
        _asteroidsNum = 0;
    }
        
    
    private async UniTask LevelSequence()
    {
        while (_isPlaying && _sleepingAsteroidModels.Count > 0)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delay)); 
        
            var asteroid = _sleepingAsteroidModels.PickRandom();
            _sleepingAsteroidModels.Remove(asteroid);
            
            SetAsteroid(asteroid, GetRandomPosition());
        }
    }
    
    private void SetAsteroid(AsteroidModel asteroid, Vector3 position)
    {
        asteroid.SetPosition(position);
        asteroid.Reset();
        _activeAsteroidModels.Add(asteroid);
    }

    private void ProcessCollapse(AsteroidCollapseSignal signal)
    {
        var size = signal.Size;
        var pos = signal.Position;
        
        if (!_isPlaying)
            return;
        
        // Todo to different classes?
        switch (size)
        {
            case AsteroidSize.Small:
                _asteroidsNum -= 1;
                CheckLevelFinished();
                break;
            case AsteroidSize.Medium:
                SetAsteroid(_asteroidsController
                    .CreateAsteroid(AsteroidSize.Small), pos);
                SetAsteroid(_asteroidsController
                    .CreateAsteroid(AsteroidSize.Small), pos);
                _asteroidsNum++;
                break;
            case AsteroidSize.Big:
                SetAsteroid(_asteroidsController
                    .CreateAsteroid(AsteroidSize.Medium), pos);
                SetAsteroid(_asteroidsController
                    .CreateAsteroid(AsteroidSize.Medium), pos);
                _asteroidsNum++;
                break;
        }
    }
    
    private Vector2 GetRandomPosition()
    {
        var x = RandomUtils.GetFloat(0, Screen.width);
        var y = RandomUtils.GetFloat(0, Screen.height);
        
        return _worldPointProvider.GetFromScreen(new Vector2(x, y));
    }
    
    private void CheckLevelFinished()
    {
        if (_asteroidsNum == 0)
        {
            _signalBus.Fire(new PauseGameSignal());
            _signalBus.Fire(new LevelPassedSignal());
            _onLevelFinished?.Invoke(_currentLevel);
        }
    }
}
}