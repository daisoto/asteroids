public abstract class AsteroidsNumProvider
{
    protected readonly int _maxLevel;
    
    public AsteroidsNumProvider(int maxLevel)
    {
        _maxLevel = maxLevel;
    }
    
    public abstract  int GetNum(int level, int maxNum);
}