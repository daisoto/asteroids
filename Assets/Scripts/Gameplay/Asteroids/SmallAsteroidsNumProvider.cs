using UnityEngine;

namespace Gameplay
{
public class SmallAsteroidsNumProvider: IAsteroidsNumProvider
{
    private readonly int _maxLevel;
    
    public SmallAsteroidsNumProvider(int maxLevel)
    {
        _maxLevel = maxLevel;
    }
    
    public int GetNum(int level, int maxNum)
    {
        var max = Mathf.RoundToInt(
            maxNum * ((_maxLevel - level) / (float)_maxLevel));
        
        var min = Mathf.RoundToInt(
            maxNum * ((_maxLevel - level * 2) / (float)_maxLevel));
        
        if (min < 0) 
            min = 0;
        
        return RandomUtils.GetInt(min, max);
    }
}
}