using UniRx;
using UnityEngine;

namespace Gameplay
{
public class SpaceshipModel: SpaceModel, IResettable
{
    private readonly HealthModel _healthModel;
    private readonly SpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<Vector3> Speed => _speedProvider.Speed;
    public IReadOnlyReactiveProperty<int> Health => _healthModel.Health;

    public SpaceshipModel(HealthModel healthModel, 
        SpeedProvider speedProvider)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
    }
    
    public void Reset()
    {
        _healthModel.Reset();
        _speedProvider.Reset();
        Activate();
    }
    
    public void UpdateSpeed(Vector3 dir) => _speedProvider.UpdateSpeed(dir);
    
    public void DecreaseHealth(int damage) => 
        _healthModel.DecreaseHealth(damage);
}
}