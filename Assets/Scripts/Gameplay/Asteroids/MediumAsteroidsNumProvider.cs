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
        var b = maxNum / _middle + _middle;
        var max = Mathf.RoundToInt(-(level * level) + b * level);
        var min = Mathf.RoundToInt(max - _step);
        
        return RandomUtils.GetInt(min, max);
    }
}
}