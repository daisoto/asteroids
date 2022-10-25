using System;
using UnityEngine;
using Zenject;
using Data;

namespace Gameplay
{
public class LevelController: IInitializable, IDisposable
{
    private readonly AsteroidsController _asteroidsController;
    private readonly Camera _camera;
    private readonly SignalBus _signalBus;
    
    private int _asteroidsNum; 
    private int _currentLevel;
    private Action<int> _onLevelFinished;

    public LevelController(AsteroidsController asteroidsController, 
        Camera camera, SignalBus signalBus)
    {
        _asteroidsController = asteroidsController;
        _camera = camera;
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<AsteroidCollapseSignal>(ProcessCollapse);
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<AsteroidCollapseSignal>(ProcessCollapse);
    }
    
    public LevelController SetOnLevelFinished(Action<int> onLevelFinished)
    {
        _onLevelFinished = onLevelFinished;
        
        return this;
    }
    
    public void StartLevel(LevelData levelData)
    {
        _asteroidsNum = 0;
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
                asteroid.UpdateSpeed();
                asteroid.SetPosition(GetRandomPosition());
            }
        }
        
        _signalBus.Fire(new ResumeGameSignal());
    }

    private void ProcessCollapse(AsteroidCollapseSignal signal)
    {
        var size = signal.Size;
        var pos = signal.Position;
        
        // Todo to different classes?
        switch (size)
        {
            case AsteroidSize.Small:
                _asteroidsNum -= 1;
                CheckLevelFinished();
                break;
            case AsteroidSize.Medium:
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Small)
                    .SetPosition(pos);
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Small)
                    .SetPosition(pos);
                _asteroidsNum++;
                break;
            case AsteroidSize.Big:
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Medium)
                    .SetPosition(pos);
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Medium)
                    .SetPosition(pos);
                _asteroidsNum++;
                break;
        }
    }
    
    private Vector2 GetRandomPosition()
    {
        var x = RandomUtils.GetFloat(0, Screen.width);
        var y = RandomUtils.GetFloat(0, Screen.height);
        return _camera.ScreenToWorldPoint(new Vector2(x, y));
    }
    
    private void CheckLevelFinished()
    {
        if (_asteroidsNum == 0)
        {
            _signalBus.Fire(new PauseGameSignal());
            _onLevelFinished?.Invoke(_currentLevel);
        }
    }
}
}