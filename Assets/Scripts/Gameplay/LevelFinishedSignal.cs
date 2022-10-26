namespace Gameplay
{
public readonly struct LevelFinishedSignal: ISignal
{ 
    public int Level { get; }
    public LevelFinishedSignal(int level)
    {
        Level = level;
        
    }
}
}