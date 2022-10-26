using UniRx;
using UnityEngine;

namespace Gameplay
{
public class SpaceshipModel: SpaceModel
{
    private readonly HealthModel _healthModel;
    private readonly ISpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<Vector3> Speed => _speedProvider.Speed;
    public IReadOnlyReactiveProperty<int> Health => _healthModel.Health;

    public SpaceshipModel(HealthModel healthModel, 
        ISpeedProvider speedProvider)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
    }
    
    public SpaceshipModel Restore()
    {
        _healthModel.Restore();
        
        return this;
    }
    
    public void UpdateSpeed(Vector3 dir) => _speedProvider.UpdateSpeed(dir);
    
    public void DecreaseHealth(int damage) => 
        _healthModel.DecreaseHealth(damage);
}
}