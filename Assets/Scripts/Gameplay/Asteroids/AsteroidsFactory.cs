using System;

public class AsteroidsFactory: IFactory<AsteroidModel, AsteroidSize>
{
    private readonly AsteroidsData _data;
    
    private Action<AsteroidModel, AsteroidSize> _onCreated;
    
    public AsteroidsFactory(AsteroidsData data)
    {
        _data = data;
    }
    
    public AsteroidModel Get(AsteroidSize size)
    {
        AsteroidModel model = null;
        
        foreach (var aData in _data.Data)
            if (aData.Size == size)
                model = Get(aData);
        
        if (model != null)
            _onCreated?.Invoke(model, size);
        
        return model;
    }

    public void SetOnCreated(Action<AsteroidModel, AsteroidSize> onCreated)
    {
        _onCreated = onCreated;
    }

    private AsteroidModel Get(AsteroidData data)
    {
        var healthModel = new HealthModel(data.MaxHealth);
        
        return new AsteroidModel(healthModel, data.Damage, data.MaxSpeed, data.MinSpeed);
    }
}