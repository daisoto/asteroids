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
    
    public void StartLevel(int level, Action onFinish)
    {
        var data = GetLevelData(level);
        _levelController
            .SetOnLevelFinished(FinishLevel)
            .StartLevel(data);
    
        void FinishLevel()
        {
            _levelsData[level].IsFinished = true;
            onFinish.Invoke();
        }
    }

    private LevelData GetLevelData(int level)
    {
        if (_levelsData.Count <= level)
            return _levelsData[level];
        
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