using UnityEngine;
using Data;

namespace Gameplay
{
public class TotalAsteroidsProvider: ITotalAsteroidsProvider
{
    private readonly AsteroidsData _asteroidsData;
    private readonly ILevelsViewDataProvider _levelsViewDataProvider;
    
    private int _minAsteroidsNum => _asteroidsData.MinAsteroidsNum;
    private int _maxAsteroidsNum => _asteroidsData.MaxAsteroidsNum;
    private int _maxLevel => _levelsViewDataProvider.MaxLevel;
    
    public TotalAsteroidsProvider(AsteroidsData asteroidsData, 
        ILevelsViewDataProvider levelsViewDataProvider)
    {
        _asteroidsData = asteroidsData;
        _levelsViewDataProvider = levelsViewDataProvider;
    }
    
    public int Get(int level) 
    {
        var lerp = Mathf.RoundToInt(
            Mathf.Lerp(_minAsteroidsNum, _maxAsteroidsNum, 
                (float)level / _maxLevel));
        var delta = (_maxAsteroidsNum - _minAsteroidsNum) / 2;
        var left = lerp - delta; 
        var right = lerp + delta;
        var random = RandomUtils.GetInt(left, right);
        
        return Mathf.Clamp(random, _minAsteroidsNum, _maxAsteroidsNum);
    }
}
}