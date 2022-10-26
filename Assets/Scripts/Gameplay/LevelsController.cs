using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using Zenject;

namespace Gameplay
{
public class LevelsController
{
    private readonly Dictionary<AsteroidSize, AsteroidsNumProvider> 
        _asteroidsNumProviders;
    private readonly LevelController _levelController;
    private readonly LevelsDataManager _levelsDataManager;
    private readonly LevelsSettings _levelsSettings;
    private readonly AsteroidsSettings _asteroidsSettings;
    private readonly SignalBus _signalBus;
    
    private readonly List<LevelData> _savedLevelsData;
    public IList<LevelData> SavedLevelsData => _savedLevelsData;
    
    private int _minAsteroidsNum => _asteroidsSettings.MinAsteroidsNum;
    private int _maxAsteroidsNum => _asteroidsSettings.MaxAsteroidsNum;

    public LevelsController(LevelController levelController,
        [Inject(Id = AsteroidSize.Small)]
        AsteroidsNumProvider smallAsteroidNumProvider, 
        [Inject(Id = AsteroidSize.Medium)]
        AsteroidsNumProvider mediumAsteroidNumProvider, 
        LevelsDataManager levelsDataManager,
        LevelsSettings levelsSettings, 
        AsteroidsSettings asteroidsSettings, SignalBus signalBus)
    {
        _levelController = levelController;
        _levelsDataManager = levelsDataManager;
        _levelsSettings = levelsSettings;
        _asteroidsSettings = asteroidsSettings;
        _signalBus = signalBus;

        _savedLevelsData = 
            levelsDataManager.TryLoad(out var levelsData) ? 
            levelsData : new List<LevelData>();
        
        _asteroidsNumProviders = 
            new Dictionary<AsteroidSize, AsteroidsNumProvider>
        {
            {AsteroidSize.Small, smallAsteroidNumProvider},
            {AsteroidSize.Medium, mediumAsteroidNumProvider}
        };
    }
    
    public void StartLevel(int level)
    {
        _signalBus.Fire(new LevelStartedSignal(level));
        var data = GetLevelData(level);
        _levelController
            .SetOnLevelFinished(FinishLevel)
            .SetDelay(_levelsSettings.GetDelay(level))
            .StartLevel(data);
    }
    
    private void FinishLevel(int level)
    {
        _savedLevelsData[level - 1].IsFinished = true;
        _levelsDataManager.Save(_savedLevelsData);
        _signalBus.Fire(new LevelFinishedSignal(level));
    }

    private LevelData GetLevelData(int level)
    {
        if (_savedLevelsData.Count >= level)
            return _savedLevelsData[level - 1];
        
        var totalNum = GetTotalAsteroidsNum(level);
        var sizes = EnumUtils.GetValues<AsteroidSize>();
        var asteroidsNum = new Dictionary<AsteroidSize, int>();
        
        foreach (var size in sizes)
        {
            var num = GetAsteroidsNum(size, level, totalNum);
            totalNum -= num;
            asteroidsNum.Add(size, num);
        }
        
        var levelData = new LevelData(level, asteroidsNum);
        _savedLevelsData.Add(levelData);
        
        _levelsDataManager.Save(_savedLevelsData);

        return levelData;
    }
    
    private int GetAsteroidsNum(AsteroidSize size, int level, int asteroidsLeft)
    {
        return _asteroidsNumProviders.ContainsKey(size) ? 
            _asteroidsNumProviders[size].GetNum(level, asteroidsLeft) 
            : asteroidsLeft;
    }
    
    private int GetTotalAsteroidsNum(int level) 
    {
        var lerp = Mathf.RoundToInt(
            Mathf.Lerp(_minAsteroidsNum, _maxAsteroidsNum, 
                (float)level / _levelsSettings.MaxLevel));
        var delta = (_maxAsteroidsNum - _minAsteroidsNum) / 2;
        var left = lerp - delta; 
        var right = lerp + delta;
        var random = RandomUtils.GetInt(left, right);
        
        return Mathf.Clamp(random, _minAsteroidsNum, _maxAsteroidsNum);
    }
}
}