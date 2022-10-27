using System;
using UniRx;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Gameplay
{
public class AsteroidsController: IDisposable
{
    private readonly Dictionary<AsteroidSize, AsteroidsPool> _pools;
    
    private readonly IFactory<AsteroidBehaviour, AsteroidSize> 
        _behavioursFactory;
    private readonly IFactory<AsteroidModel, AsteroidSize> _modelsFactory;
    
    private readonly SignalBus _signalBus;
    
    private readonly List<AsteroidController> _controllers;
    
    private readonly DisposablesContainer _disposablesContainer;

    public AsteroidsController(SignalBus signalBus,
        IFactory<AsteroidBehaviour, AsteroidSize> behavioursFactory,
        IFactory<AsteroidModel, AsteroidSize> modelsFactory)
    {
        _signalBus = signalBus;
        _behavioursFactory = behavioursFactory;
        _modelsFactory = modelsFactory
            .SetOnCreated(OnModelCreated);
        
        _pools = GetPools();
        
        _controllers = new List<AsteroidController>();
        _disposablesContainer = new DisposablesContainer();
    }
    
    public void Dispose()
    {
        _disposablesContainer.Dispose();
        _controllers.ForEach(c => c.Dispose());
    }
    
    public AsteroidModel CreateAsteroid(AsteroidSize size) => 
        _pools[size].Get();

    private void OnModelCreated(AsteroidModel model, AsteroidSize size)
    {
        var behaviour = _behavioursFactory.Get(size);
        
        var controller = new AsteroidController(model, behaviour, size)
            .SetOnDeactivate(OnDeactivateModel)
            .SetOnDestroy(OnDestroyModel);
        
        _controllers.Add(controller);
    }
    
    private Dictionary<AsteroidSize, AsteroidsPool> GetPools()
    {
        return new Dictionary<AsteroidSize, AsteroidsPool>
        {
            { 
                AsteroidSize.Small, new AsteroidsPool(
                    new Factory<AsteroidModel>(GetSmallModel))
            },
            { 
                AsteroidSize.Medium, new AsteroidsPool(
                    new Factory<AsteroidModel>(GetMediumModel))
            },
            { 
                AsteroidSize.Big, new AsteroidsPool(
                    new Factory<AsteroidModel>(GetBigModel))
            }
        };
    }
    
    private AsteroidModel GetSmallModel() => _modelsFactory.Get(AsteroidSize.Small);
    private AsteroidModel GetMediumModel() => _modelsFactory.Get(AsteroidSize.Medium);
    private AsteroidModel GetBigModel() => _modelsFactory.Get(AsteroidSize.Big);
    
    private void OnDeactivateModel(AsteroidSize size, AsteroidModel model) => 
        _pools[size].Return(model);
    
    private void OnDestroyModel(AsteroidSize size, Vector3 position) => 
        _signalBus.Fire(new AsteroidCollapseSignal(size, position));
}
}