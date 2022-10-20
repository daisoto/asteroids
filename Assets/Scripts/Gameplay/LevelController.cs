using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LevelController: IInitializable, IDisposable
{
    private readonly Dictionary<AsteroidSize, AsteroidsNumProvider> _asteroidsNumProviders;
    private readonly AsteroidsController _asteroidsController;
    private readonly Camera _camera;
    private readonly SignalBus _signalBus;
    private readonly int _level;
    private readonly int _asteroidsNum;
    
    public LevelController(AsteroidsController asteroidsController, Camera camera,
        [Inject(Id = AsteroidSize.Small)]
        AsteroidsNumProvider smallAsteroidNumProvider, 
        [Inject(Id = AsteroidSize.Medium)]
        AsteroidsNumProvider mediumAsteroidNumProvider, 
        SignalBus signalBus, int level, int asteroidsNum)
    {
        _asteroidsController = asteroidsController;
        _camera = camera;
        
        _asteroidsNumProviders = new Dictionary<AsteroidSize, AsteroidsNumProvider>
        {
            {AsteroidSize.Small, smallAsteroidNumProvider},
            {AsteroidSize.Medium, mediumAsteroidNumProvider}
        };

        _signalBus = signalBus;
        _level = level;
        _asteroidsNum = asteroidsNum;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<AsteroidCollapseSignal>(ProcessCollapse);
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<AsteroidCollapseSignal>(ProcessCollapse);
    }
    
    public void CreateAsteroids()
    {
        var sizes = Enum.GetValues(typeof(AsteroidSize)).Cast<AsteroidSize>();
        var asteroidsNum = _asteroidsNum;
        
        foreach (var size in sizes)
        {
            var num = asteroidsNum;
            if (_asteroidsNumProviders.ContainsKey(size))
            {
                num = _asteroidsNumProviders[size].GetNum(_level, asteroidsNum); 
                asteroidsNum -= num;
            }
            
            for (int i = 0; i < num; i++)
                _asteroidsController
                    .CreateAsteroid(size)
                    .SetPosition.Execute(GetRandomPosition());
        }
    }

    private void ProcessCollapse(AsteroidCollapseSignal signal)
    {
        switch (signal.Size)
        {
            case AsteroidSize.Small:
                // todo?
            break;
            case AsteroidSize.Medium:
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Small)
                    .SetPosition.Execute(signal.Position);
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Small)
                    .SetPosition.Execute(signal.Position);
            break;
            case AsteroidSize.Big:
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Medium)
                    .SetPosition.Execute(signal.Position);
                _asteroidsController
                    .CreateAsteroid(AsteroidSize.Medium)
                    .SetPosition.Execute(signal.Position);
            break;
        }
    }
    
    private Vector2 GetRandomPosition()
    {
        var x = RandomUtility.GetFloat(0, Screen.width);
        var y = RandomUtility.GetFloat(0, Screen.height);
        return _camera.ScreenToWorldPoint(new Vector2(x, y));
    }
}