using Data;

namespace Gameplay
{
public class AsteroidsFactory: Factory<AsteroidModel, AsteroidSize>
{
    private readonly AsteroidsData _data;
    public AsteroidsFactory(AsteroidsData data)
    {
        _data = data;
    }

    protected override AsteroidModel Create(AsteroidSize size)
    {
        AsteroidModel model = default;
        
        foreach (var aData in _data.Data)
            if (aData.Size == size)
                model = CreateModel(aData);
        
        return model;
    }

    private AsteroidModel CreateModel(AsteroidData data)
    {
        var healthModel = new HealthModel(data.MaxHealth);
        var speedModel = new RandomSpeedModel(data.MaxSpeed, data.MinSpeed);
        
        return new AsteroidModel(healthModel, speedModel, data.Damage);
    }
}
}