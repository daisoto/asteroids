namespace Data
{
public readonly struct SetSpaceshipDataSignal: ISignal
{
    public SpaceshipData Data { get; }
    public bool IsNew { get; }
    
    public SetSpaceshipDataSignal(SpaceshipData data, bool isNew = false)
    {
        IsNew = isNew;
        Data = data;
    }
}
}