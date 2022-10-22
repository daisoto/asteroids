using UniRx;
using UnityEngine;

public class SpaceshipModel
{
    private readonly HealthModel _healthModel;
    private readonly ISpeedProvider _speedProvider;
    
    public Texture2D Texture { get; }
    public float Speed => _speedProvider.Speed.Value;
    public IReadOnlyReactiveProperty<int> Health => _healthModel.Health;
    public int MaxHealth => _healthModel.MaxHealth;

    public SpaceshipModel(HealthModel healthModel, 
        ISpeedProvider speedProvider,
        Texture2D texture)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
        
        Texture = texture;
    }
    
    public void DecreaseHealth(int damage) => 
        _healthModel.DecreaseHealth(damage);
}