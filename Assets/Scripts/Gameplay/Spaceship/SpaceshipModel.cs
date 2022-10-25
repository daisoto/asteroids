using UniRx;

namespace Gameplay
{
public class SpaceshipModel: ISpeedProvider
{
    private readonly HealthModel _healthModel;
    private readonly ISpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<float> Speed => _speedProvider.Speed;
    public IReadOnlyReactiveProperty<int> Health => _healthModel.Health;

    public SpaceshipModel(HealthModel healthModel, 
        ISpeedProvider speedProvider)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
    }
    
    public void UpdateSpeed() => _speedProvider.UpdateSpeed();
    
    public void DecreaseHealth(int damage) => 
        _healthModel.DecreaseHealth(damage);
}
}