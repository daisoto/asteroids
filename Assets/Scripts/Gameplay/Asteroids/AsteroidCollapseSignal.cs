using UnityEngine;

public readonly struct AsteroidCollapseSignal: ISignal
{
    public AsteroidSize Size { get; }
    public Vector3 Position { get; }
    
    public AsteroidCollapseSignal(AsteroidSize size, Vector3 position)
    {
        Size = size;
        Position = position;
    }
}