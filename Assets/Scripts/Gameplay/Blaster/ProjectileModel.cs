using UniRx;
using UnityEngine;

namespace Gameplay
{
public class ProjectileModel: SpaceModel
{
    private readonly ISpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<Vector3> Speed => _speedProvider.Speed;
    
    public ProjectileModel(ISpeedProvider speedProvider)
    {
        _speedProvider = speedProvider;
    }
    
    public void UpdateSpeed(Vector3 dir) => _speedProvider.UpdateSpeed(dir);
}
}