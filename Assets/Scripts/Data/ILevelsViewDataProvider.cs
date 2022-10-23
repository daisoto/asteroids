using UnityEngine;

namespace Data
{
public interface ILevelsViewDataProvider
{
    int MaxLevel { get; }
    
    Color OpenedColor { get; }
    
    Color ClosedColor { get; }
    
    Color FinishedColor { get; }
}
}