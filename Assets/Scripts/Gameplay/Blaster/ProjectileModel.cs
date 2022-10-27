using UniRx;
using UnityEngine;

namespace Gameplay
{
public class ProjectileModel: SpaceModel, IResettable
{
    private readonly SpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<Vector3> Speed => _speedProvider.Speed;
    
    public Quaternion InitialRotation { get; set; }
    
    public ProjectileModel(SpeedProvider speedProvider)
    {
        _speedProvider = speedProvider;
    }
    
    public void UpdateSpeed(Vector3 dir) => _speedProvider.UpdateSpeed(dir);
    
    public void Reset()
    {
        _speedProvider.Reset();
        Activate();
    }
}
}