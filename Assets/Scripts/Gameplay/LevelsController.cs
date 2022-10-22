using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelsController: IInitializable, IDisposable
{
    private readonly Dictionary<AsteroidSize, AsteroidsNumProvider> _asteroidsNumProviders;
    private readonly AsteroidsController _asteroidsController;
    private readonly LevelsDataManager _levelsDataManager;
    private readonly ITotalAsteroidsProvider _totalAsteroidsProvider;
    private readonly Camera _camera;
    private readonly SignalBus _signalBus;
    
    private readonly List<LevelData> _levels;
    
    private int _currentLevel;
    
    public LevelsController(AsteroidsController asteroidsController, Camera camera,
        [Inject(Id = AsteroidSize.Small)]
        AsteroidsNumProvider smallAsteroidNumProvider, 
        [Inject(Id = AsteroidSize.Medium)]
        AsteroidsNumProvider mediumAsteroidNumProvider, 
        LevelsDataManager levelsDataManager,
        ITotalAsteroidsProvider totalAsteroidsProvider,
        SignalBus signalBus)
    {
        _asteroidsController = asteroidsController;
        _totalAsteroidsProvider = totalAsteroidsProvider;
        _levelsDataManager = levelsDataManager;
        
        _levels = 
            levelsDataManager.TryLoad(out var levelsData) ? 
            levelsData : new List<LevelData>();
        
        _camera = camera;
        
        _asteroidsNumProviders = new Dictionary<AsteroidSize, AsteroidsNumProvider>
        {
            {AsteroidSize.Small, smallAsteroidNumProvider},
            {AsteroidSize.Medium, mediumAsteroidNumProvider}
        };

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
    
    public LevelsController SetLevel(int level)
    {
        _currentLevel = level;
        
        return this;
    }
    
    public void CreateAsteroids()
    {
        var data  = GetLevelData();
        var sizes = EnumUtils.GetValues<AsteroidSize>();
        foreach (var size in sizes)
        {
            var num = data.AsteroidsNum[size];
            
            for (int i = 0; i < num; i++)
                _asteroidsController
                    .CreateAsteroid(size)
                    .SetPosition.Execute(GetRandomPosition());
        }
    }
    
    private LevelData GetLevelData()
    {
        if (_levels.Count <= _currentLevel)
            return _levels[_currentLevel];
        
        var totalNum = _totalAsteroidsProvider.Get(_currentLevel);
        var sizes = EnumUtils.GetValues<AsteroidSize>();
        var asteroidsNum = new Dictionary<AsteroidSize, int>();
        
        foreach (var size in sizes)
        {
            var num = GetAsteroidsNum(size, totalNum);
            totalNum -= num;
            asteroidsNum.Add(size, num);
        }
        
        var levelData = new LevelData { AsteroidsNum = asteroidsNum };
        _levels.Add(levelData);
        
        _levelsDataManager.Save(_levels);

        return levelData;
    }
    
    private int GetAsteroidsNum(AsteroidSize size, int asteroidsLeft)
    {
        return _asteroidsNumProviders.ContainsKey(size) ? 
            _asteroidsNumProviders[size].GetNum(_currentLevel, asteroidsLeft) 
            : asteroidsLeft;
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
        var x = RandomUtils.GetFloat((float) 0, Screen.width);
        var y = RandomUtils.GetFloat((float) 0, Screen.height);
        return _camera.ScreenToWorldPoint(new Vector2(x, y));
    }
}