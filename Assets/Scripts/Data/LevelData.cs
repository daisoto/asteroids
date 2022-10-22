using System.Collections.Generic;

public struct LevelData
{
    public Dictionary<AsteroidSize, int> AsteroidsNum { get; set; }
    public bool IsFinished { get; set; }
}