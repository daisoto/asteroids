using UnityEngine;

namespace Gameplay
{
public class MediumAsteroidsNumProvider: IAsteroidsNumProvider
{
    private readonly float _middle;
    private readonly float _step;
    
    public MediumAsteroidsNumProvider(int maxLevel)
    {
        _middle  = maxLevel / 2f;
        _step = maxLevel / 4f;
    }
    
    public int GetNum(int level, int maxNum)
    {
        var b = 2 * _middle;
        var max = Mathf.RoundToInt(-(level * level) + b * level);
        var min = Mathf.RoundToInt(max - _step);
        
        if (min < 0) 
            min = 0;
        
        return RandomUtils.GetInt(min, max);
    }
}
}