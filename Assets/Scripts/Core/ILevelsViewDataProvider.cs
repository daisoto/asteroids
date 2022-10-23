using UnityEngine;

public interface ILevelsViewDataProvider
{
    int MaxLevel { get; }
    
    Color OpenedColor { get; }
    
    Color ClosedColor { get; }
    
    Color FinishedColor { get; }
}