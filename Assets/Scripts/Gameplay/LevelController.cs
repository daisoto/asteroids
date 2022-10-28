using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Zenject;
using Data;

namespace Gameplay
{
public class LevelController: IInitializable, IDisposable
{
    private readonly AsteroidsController _asteroidsController;
    private readonly IWorldPointProvider _worldPointProvider;
    private readonly CameraShaker _cameraShaker;
    private readonly SignalBus _signalBus;
    
    private readonly List<AsteroidModel> _sleepingAsteroidModels;
    private readonly List<AsteroidModel> _activeAsteroidModels;
    
    private int _asteroidsNum; 
    private int _currentLevel;
    private float _delay;
    // private bool _isPlaying;
    private CancellationDisposable _playingCancellation; 
    private Action<int> _onLevelFinished;

    public LevelController(AsteroidsController asteroidsController, 
        IWorldPointProvider worldPointProvider, 
        CameraShaker cameraShaker, SignalBus signalBus)
    {
        _asteroidsController = asteroidsController;
        _worldPointProvider = worldPointProvider;
        _signalBus = signalBus;
        _cameraShaker = cameraShaker;

        _sleepingAsteroidModels = new List<AsteroidModel>();
        _activeAsteroidModels = new List<AsteroidModel>();
    }

    public void Initialize()
    {
        _signalBus.Subscribe<SpaceshipDestroyedSignal>(StopPlaying);
        _signalBus.Subscribe<LevelEndedSignal>(OnLevelEnd);
        
        _asteroidsController
            .SetOnExplode(ProcessCollapse)
            .SetOnDeactivate(RemoveFromList);
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<SpaceshipDestroyedSignal>(StopPlaying);
        _signalBus.Unsubscribe<LevelEndedSignal>(OnLevelEnd);
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
        _playingCancellation = new CancellationDisposable();
        _currentLevel = levelData.Level;
        
        var sizes = EnumUtils.GetValues<AsteroidSize>();
        foreach (var size in sizes)
        {
            var num = levelData.AsteroidsNum[size];
            _asteroidsNum += num;
            
            for (int i = 0; i < num; i++)
            {
                var asteroid = _asteroidsController.Get(size);
                _sleepingAsteroidModels.Add(asteroid);
            }
        }
        
        _signalBus.Fire(new ResumeGameSignal());
        LevelSequence().Forget();
    }
    
    private void OnLevelEnd()
    {
        StopPlaying();
        DeactivateRemaining();
    }
    
    private void StopPlaying() => _playingCancellation?.Dispose();
    
    private void DeactivateRemaining() 
    {
        while (_activeAsteroidModels.Count > 0)
            _activeAsteroidModels[0].Deactivate();
        
        while (_sleepingAsteroidModels.Count > 0)
            _sleepingAsteroidModels[0].Deactivate();
        
        _asteroidsNum = 0;
    }
        
    
    private async UniTask LevelSequence()
    {
        while (_sleepingAsteroidModels.Count > 0)
        {
            var asteroid = _sleepingAsteroidModels.PickRandom();
            await UniTask.Delay(TimeSpan.FromSeconds(_delay), 
                cancellationToken: _playingCancellation.Token);
            
            _sleepingAsteroidModels.Remove(asteroid);
            SetAsteroid(asteroid, GetRandomPosition());
        }
    }
    
    private void SetAsteroid(AsteroidModel asteroid, Vector3 position)
    {
        asteroid.SetPosition(position);
        asteroid.Reset();
        _activeAsteroidModels.Add(asteroid);
        
        var sub = asteroid.IsActive
            .Subscribe(isActive =>
            {
                if (!isActive)
                    _activeAsteroidModels.Remove(asteroid);
            });
        
    }

    private void ProcessCollapse(AsteroidModel model)
    {
        var size = model.Size;
        var pos = model.Position.Value;
        
        if (model.ExplosionStrength != Vector3.zero)
            _cameraShaker.Shake(_delay, model.ExplosionStrength);
        
        switch (size)
        {
            case AsteroidSize.Small:
                _asteroidsNum -= 1;
                CheckLevelFinished();
                break;
            case AsteroidSize.Medium:
                SetAsteroid(_asteroidsController.Get(AsteroidSize.Small), pos);
                SetAsteroid(_asteroidsController.Get(AsteroidSize.Small), pos);
                _asteroidsNum++;
                break;
            case AsteroidSize.Big:
                SetAsteroid(_asteroidsController.Get(AsteroidSize.Medium), pos);
                SetAsteroid(_asteroidsController.Get(AsteroidSize.Medium), pos);
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
    
    private void RemoveFromList(AsteroidModel model) 
    {
        if (_activeAsteroidModels.Contains(model))
            _activeAsteroidModels.Remove(model);
        
        if (_sleepingAsteroidModels.Contains(model))
            _sleepingAsteroidModels.Remove(model);
    }
}
}