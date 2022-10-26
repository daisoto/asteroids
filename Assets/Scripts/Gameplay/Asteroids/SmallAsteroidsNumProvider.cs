using UnityEngine;

namespace Gameplay
{
public class SmallAsteroidsNumProvider: AsteroidsNumProvider
{
    public SmallAsteroidsNumProvider(int maxLevel) : base(maxLevel) { }
    
    public override int GetNum(int level, int maxNum)
    {
        var minNum = Mathf.RoundToInt(
            (_maxLevel - level + 1f) / (_maxLevel + 1f) * maxNum);
        
        return RandomUtils.GetInt(minNum, maxNum);
    }
}
}