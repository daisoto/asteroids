using UniRx;

namespace Gameplay
{
public class AsteroidModel: SpaceModel, ISpeedProvider
{
    public int Damage { get; }

    private readonly HealthModel _healthModel;
    private readonly ISpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<float> Speed => _speedProvider.Speed;
    
    public AsteroidModel(HealthModel healthModel,  
        ISpeedProvider speedProvider, int damage)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
        Damage = damage;
        
        _healthModel.SetOnDeath(Deactivate);
    }
    
    public void UpdateSpeed() => _speedProvider.UpdateSpeed();
    
    public void DecreaseHealth(int damage) => 
        _healthModel.DecreaseHealth(damage);
}
}