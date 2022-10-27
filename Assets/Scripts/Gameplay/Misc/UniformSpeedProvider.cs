using UniRx;
using UnityEngine;

namespace Gameplay
{
public class UniformSpeedProvider: SpeedProvider
{
    public override IReadOnlyReactiveProperty<Vector3> Speed => _speed;
    private  readonly ReactiveProperty<Vector3> _speed;
    
    private readonly float _speedInternal;
    
    public UniformSpeedProvider(float speed)
    {
        _speed = new ReactiveProperty<Vector3>();
        _speedInternal = speed;
    }
    
    public override void UpdateSpeed(Vector3 direction) => 
        _speed.SetValueAndForceNotify(direction * _speedInternal);
}
}