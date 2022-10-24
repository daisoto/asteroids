using UniRx;

namespace Gameplay
{
public class SpaceshipModel
{
    private readonly HealthModel _healthModel;
    private readonly ISpeedProvider _speedProvider;
    
    public float Speed => _speedProvider.Speed.Value;
    public IReadOnlyReactiveProperty<int> Health => _healthModel.Health;

    public SpaceshipModel(HealthModel healthModel, 
        ISpeedProvider speedProvider)
    {
        _healthModel = healthModel;
        _speedProvider = speedProvider;
    }
    
    public void DecreaseHealth(int damage) => 
        _healthModel.DecreaseHealth(damage);
}
}