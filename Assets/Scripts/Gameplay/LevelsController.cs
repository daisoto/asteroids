using System;
using System.Collections.Generic;
using Data;
using Zenject;

namespace Gameplay
{
public class LevelsController
{
    private readonly Dictionary<AsteroidSize, AsteroidsNumProvider> 
        _asteroidsNumProviders;
    private readonly LevelController _levelController;
    private readonly LevelsDataManager _levelsDataManager;
    private readonly ITotalAsteroidsProvider _totalAsteroidsProvider;
    
    private readonly List<LevelData> _levelsData;
    public IList<LevelData> LevelsData => _levelsData;
    
    private Action _onLevelStart; 
    private Action _onLevelFinished; 
    private Action<int> _onSpecificLevelFinished;

    public LevelsController(LevelController levelController,
        [Inject(Id = AsteroidSize.Small)]
        AsteroidsNumProvider smallAsteroidNumProvider, 
        [Inject(Id = AsteroidSize.Medium)]
        AsteroidsNumProvider mediumAsteroidNumProvider, 
        LevelsDataManager levelsDataManager,
        ITotalAsteroidsProvider totalAsteroidsProvider)
    {
        _levelController = levelController;
        _levelsDataManager = levelsDataManager;
        _totalAsteroidsProvider = totalAsteroidsProvider;

        _levelsData = 
            levelsDataManager.TryLoad(out var levelsData) ? 
            levelsData : new List<LevelData>();
        
        _asteroidsNumProviders = 
            new Dictionary<AsteroidSize, AsteroidsNumProvider>
        {
            {AsteroidSize.Small, smallAsteroidNumProvider},
            {AsteroidSize.Medium, mediumAsteroidNumProvider}
        };
    }
    
    public LevelsController SetOnLevelStart(Action onLevelStart)
    {
        _onLevelStart = onLevelStart;
        
        return this;
    }
    
    public LevelsController SetOnLevelFinished(Action onLevelFinished)
    {
        _onLevelFinished = onLevelFinished;
        
        return this;
    }
    
    public void StartLevel(int level, Action<int> onFinish)
    {
        _onLevelStart?.Invoke();
        
        _onSpecificLevelFinished = onFinish;
        
        var data = GetLevelData(level);
        _levelController
            .SetOnLevelFinished(FinishLevel)
            .StartLevel(data);
    }
    
    private void FinishLevel(int level)
    {
        _levelsData[level].IsFinished = true;
        _levelsDataManager.Save(_levelsData);
        
        _onSpecificLevelFinished?.Invoke(level);
        _onLevelFinished?.Invoke();
    }

    private LevelData GetLevelData(int level)
    {
        if (_levelsData.Count >= level)
            return _levelsData[level - 1];
        
        var totalNum = _totalAsteroidsProvider.Get(level);
        var sizes = EnumUtils.GetValues<AsteroidSize>();
        var asteroidsNum = new Dictionary<AsteroidSize, int>();
        
        foreach (var size in sizes)
        {
            var num = GetAsteroidsNum(size, level, totalNum);
            totalNum -= num;
            asteroidsNum.Add(size, num);
        }
        
        var levelData = new LevelData(level, asteroidsNum);
        _levelsData.Add(levelData);
        
        _levelsDataManager.Save(_levelsData);

        return levelData;
    }
    
    private int GetAsteroidsNum(AsteroidSize size, int level, int asteroidsLeft)
    {
        return _asteroidsNumProviders.ContainsKey(size) ? 
            _asteroidsNumProviders[size].GetNum(level, asteroidsLeft) 
            : asteroidsLeft;
    }
}
}