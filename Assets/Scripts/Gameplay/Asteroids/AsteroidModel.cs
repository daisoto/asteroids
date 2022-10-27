using UniRx;
using UnityEngine;

namespace Gameplay
{
public class AsteroidModel: SpaceModel
{
    private readonly HealthModel _healthModel;
    private readonly ISpeedProvider _speedProvider;
    
    public readonly ReactiveCommand Destroy;
    public readonly ReactiveCommand Explode;
    
    public IReadOnlyReactiveProperty<Vector3> Speed => _speedProvider.Speed;
    
    public int Damage { get; }
    
    public AsteroidModel(HealthModel healthModel,  
        ISpeedProvider speedProvider, int damage)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
        Damage = damage;
        
        Destroy = new ReactiveCommand();
        Explode = new ReactiveCommand();
        
        _healthModel.SetOnDeath(OnDeath);
    }
    
    public void UpdateSpeed(Vector3 dir) => _speedProvider.UpdateSpeed(dir);
    
    public void DecreaseHealth(int damage)
    {
        _healthModel.DecreaseHealth(damage);
        Explode.Execute();
    }
    
    private void OnDeath()
    {
        Destroy.Execute();
        Deactivate();
    }
}
}