using System;
using UnityEngine;
using Zenject;
using Data;

namespace Gameplay
{
public class LevelController: IInitializable, IDisposable
{
    private readonly AsteroidsController _asteroidsController;
    private readonly PlayerController _playerController;
    private readonly Camera _camera;
    private readonly SignalBus _signalBus;
    
    private int _asteroidsNum; 
    private Action _onLevelFinished;

    public LevelController(AsteroidsController asteroidsController, 
        PlayerController playerController, Camera camera, SignalBus signalBus)
    {
        _asteroidsController = asteroidsController;
        _playerController = playerController;
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
    
    public LevelController SetOnLevelFinished(Action onLevelFinished)
    {
        _onLevelFinished = onLevelFinished;
        
        return this;
    }
    
    public void StartLevel(LevelData levelData)
    {
        _asteroidsNum = 0;
        var sizes = EnumUtils.GetValues<AsteroidSize>();
        foreach (var size in sizes)
        {
            var num = levelData.AsteroidsNum[size];
            _asteroidsNum += num;
            
            for (int i = 0; i < num; i++)
                _asteroidsController
                    .CreateAsteroid(size)
                    .SetPosition.Execute(GetRandomPosition());
        }
        
        ResumeGame();
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0;
        _playerController.SetActive(false);
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1;
        _playerController.SetActive(true);
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
                    .SetPosition.Execute(pos);
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Small)
                    .SetPosition.Execute(pos);
                _asteroidsNum++;
                break;
            case AsteroidSize.Big:
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Medium)
                    .SetPosition.Execute(pos);
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Medium)
                    .SetPosition.Execute(pos);
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
            PauseGame();
            _onLevelFinished?.Invoke();
        }
    }
}
}