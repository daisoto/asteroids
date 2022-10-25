using UniRx;

namespace Gameplay
{
public class ProjectileModel: PositionableModel, ISpeedProvider
{
    private readonly ISpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<float> Speed => _speedProvider.Speed;
    
    public ProjectileModel(ISpeedProvider speedProvider)
    {
        _speedProvider = speedProvider;
    }
    
    public void UpdateSpeed() => _speedProvider.UpdateSpeed();
}
}