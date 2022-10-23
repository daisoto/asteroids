using System.Collections.Generic;

public class LevelData
{
    public int Level { get; }
    public bool IsFinished { get; set; }
    public Dictionary<AsteroidSize, int> AsteroidsNum { get; }
    
    public LevelData(int level, Dictionary<AsteroidSize, int> asteroidsNum)
    {
        Level = level;
        AsteroidsNum = asteroidsNum;
            
        IsFinished = false;
    }
}