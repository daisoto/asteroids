using System;
using UniRx;
using UnityEngine;

namespace Gameplay
{
public class AsteroidModel: SpaceModel
{
    public int Damage { get; }

    private readonly HealthModel _healthModel;
    private readonly ISpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<Vector3> Speed => _speedProvider.Speed;
    
    private Action _onDamage;
    
    public AsteroidModel(HealthModel healthModel,  
        ISpeedProvider speedProvider, int damage)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
        Damage = damage;
        
        _healthModel.SetOnDeath(Deactivate);
    }
    
    public AsteroidModel SetOnDamage(Action onDamage)
    {
        _onDamage = onDamage;
        
        return this;
    }
    
    public void UpdateSpeed(Vector3 dir) => _speedProvider.UpdateSpeed(dir);
    
    public void DecreaseHealth(int damage)
    {
        _healthModel.DecreaseHealth(damage);
        _onDamage?.Invoke();
    }
}
}