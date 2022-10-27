using Data;

namespace Gameplay
{
public class AsteroidsFactory: IFactory<AsteroidModel, AsteroidSize>
{
    private readonly AsteroidsSettings _settings;
    public AsteroidsFactory(AsteroidsSettings settings)
    {
        _settings = settings;
    }

    public AsteroidModel Get(AsteroidSize size)
    {
        AsteroidModel model = default;
        
        foreach (var aData in _settings.Data)
            if (aData.Size == size)
                model = CreateModel(aData);
        
        return model;
    }

    private AsteroidModel CreateModel(AsteroidData data)
    {
        var healthModel = new HealthModel(data.MaxHealth);
        var speedModel = new RandomSpeedModel(data.MaxSpeed, data.MinSpeed);
        var model = new AsteroidModel(
            healthModel, speedModel, data.Damage, data.Size);
        
        return model;
    }
}
}