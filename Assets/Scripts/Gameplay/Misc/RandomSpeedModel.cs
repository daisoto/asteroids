using UniRx;
using UnityEngine;

namespace Gameplay
{
public class RandomSpeedModel: SpeedProvider
{
    private readonly ReactiveProperty<Vector3> _speed;
    public override IReadOnlyReactiveProperty<Vector3> Speed => _speed;

    private readonly float _maxSpeed;
    private readonly float _minSpeed;
    
    public RandomSpeedModel(float maxSpeed, float minSpeed)
    {
        _maxSpeed = maxSpeed;
        _minSpeed = minSpeed;
        
        _speed = new ReactiveProperty<Vector3>();
    }

    public override void UpdateSpeed(Vector3 direction) => 
        _speed.Value = direction * RandomUtils.GetFloat(_minSpeed, _maxSpeed);
}
}