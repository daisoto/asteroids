using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay
{
public class AsteroidModel: IInitializable
{
    public int Damage { get; }

    private readonly HealthModel _healthModel;
    private readonly PositionableModel _positionableModel;
    private readonly ISpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<float> Speed => _speedProvider.Speed;
    public ReactiveCommand<Vector2> SetPosition => _positionableModel.SetPosition; 
    public IReadOnlyReactiveProperty<bool> IsActive => _positionableModel.IsActive;
    
    public AsteroidModel(HealthModel healthModel,  ISpeedProvider speedProvider, int damage)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
        Damage = damage;
        
        _positionableModel = new PositionableModel();
        
        _healthModel.SetOnDeath(SetDead);
    }

    public void Initialize()
    {
        _positionableModel.Initialize();
        _speedProvider.Initialize();
    }
    
    public void DecreaseHealth(int damage) => _healthModel.DecreaseHealth(damage);
    
    private void SetDead() => _positionableModel.Deactivate();
}
}