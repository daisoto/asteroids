using System;
using UniRx;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class AsteroidsController: IDisposable
{
    private readonly Dictionary<AsteroidSize, AsteroidsPool> _pools;
    
    private readonly IFactory<AsteroidBehaviour, AsteroidSize> _behavioursFactory;
    private readonly IFactory<AsteroidModel, AsteroidSize> _modelsFactory;
    
    private readonly SignalBus _signalBus;
    
    private readonly DisposablesContainer _disposablesContainer;

    public AsteroidsController(SignalBus signalBus,
        IFactory<AsteroidBehaviour, AsteroidSize> behavioursFactory,
        IFactory<AsteroidModel, AsteroidSize> modelsFactory)
    {
        _signalBus = signalBus;
        _behavioursFactory = behavioursFactory;
        _modelsFactory = modelsFactory;
        
        _modelsFactory.SetOnCreated(CreateBehaviour);
        
        _pools = new Dictionary<AsteroidSize, AsteroidsPool>
        {
            {AsteroidSize.Small, new AsteroidsPool(GetSmallModel)},
            {AsteroidSize.Medium, new AsteroidsPool(GetMediumModel)},
            {AsteroidSize.Big, new AsteroidsPool(GetBigModel)}
        };
        
        _disposablesContainer = new DisposablesContainer();
    }
    
    public void Dispose() => _disposablesContainer.Dispose();
    
    public AsteroidModel CreateAsteroid(AsteroidSize size)
    {
        return _pools[size].Get();
    }

    private void CreateBehaviour(AsteroidModel model, AsteroidSize size)
    {
        var behaviour = _behavioursFactory
            .Get(size)
            .SetDamage(model.Damage)
            .SetOnDamage(model.DecreaseHealth);
        
        _disposablesContainer.Add(model.IsAlive.Subscribe(isAlive =>
        {
            behaviour.SetActive(isAlive);
            
            if (!isAlive)
            {
                var position = behaviour.Position;
                _signalBus.Fire(new AsteroidCollapseSignal(size, position));
            }
        }));
        
        _disposablesContainer.Add(model.Speed.Subscribe(speed =>
        {
            behaviour.SetSpeed(Vector2.one * speed);
        }));
        
        _disposablesContainer.Add(model.SetPosition.Subscribe(position =>
        {
            behaviour.Position = position;
        }));
    }
    
    private AsteroidModel GetSmallModel() => _modelsFactory.Get(AsteroidSize.Small);
    private AsteroidModel GetMediumModel() => _modelsFactory.Get(AsteroidSize.Medium);
    private AsteroidModel GetBigModel() => _modelsFactory.Get(AsteroidSize.Big);
    
}
