public abstract class AsteroidsNumProvider
{
    private readonly ILevelsViewDataProvider _levelsViewDataProvider;
    protected int _maxLevel => _levelsViewDataProvider.MaxLevel;

    protected AsteroidsNumProvider(ILevelsViewDataProvider levelsViewDataProvider)
    {
        _levelsViewDataProvider = levelsViewDataProvider;
    }
    
    public abstract int GetNum(int level, int maxNum);
}