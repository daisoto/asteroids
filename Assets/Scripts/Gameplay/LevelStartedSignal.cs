namespace Gameplay
{
public readonly struct LevelStartedSignal: ISignal
{
    public int Level { get; }
    
    public LevelStartedSignal(int level)
    {
        Level = level;
    }
}
}