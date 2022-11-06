using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
public class AsteroidsManager: IDisposable
{
    private readonly IPool<AsteroidModel, AsteroidSize> _pool;
    
    private readonly IFactory<AsteroidBehaviour, AsteroidSize> 
        _behavioursFactory;
    
    private Action<AsteroidModel> _onExplode;
    private Action<AsteroidModel> _onDeactivate;
    
    private DisposablesContainer _disposablesContainer;

    public AsteroidsManager(
        IFactory<AsteroidBehaviour, AsteroidSize> behavioursFactory,
        IFactory<AsteroidModel, AsteroidSize> modelsFactory)
    {
        _behavioursFactory = behavioursFactory;
        
        var dModelsFactory =  
            new DecoratedFactory<AsteroidModel, AsteroidSize>(modelsFactory)
                .SetOnCreated(OnModelCreated);
        
        _pool = new AsteroidsPool(dModelsFactory);
        
        _disposablesContainer = new DisposablesContainer();
    }
    
    public void Dispose() 
    {
        _disposablesContainer.Dispose();
        _pool.Clear();
    }
    
    public AsteroidsManager SetOnExplode(Action<AsteroidModel> onExplode)
    {
        _onExplode = onExplode;
        
        return this;
    }
    
    public AsteroidsManager SetOnDeactivate(Action<AsteroidModel> onDeactivate)
    {
        _onDeactivate = onDeactivate;
        
        return this;
    }
    
    public AsteroidModel Get(AsteroidSize size) => 
        _pool.Get(size);

    private void OnModelCreated(AsteroidModel model, AsteroidSize size)
    {
        var behaviour = _behavioursFactory.Get(size);
        
        var controller = new AsteroidController(model, behaviour)
            .SetOnDeactivate(OnDeactivateModel)
            .SetOnExplode(OnExplode);
        
        _disposablesContainer.Add(controller);
    }
    
    private void OnDeactivateModel(AsteroidModel model) 
    {
        _onDeactivate?.Invoke(model);
        _pool.Return(model, model.Size);
    }
    
    private void OnExplode(AsteroidModel model) => 
        _onExplode?.Invoke(model);
}
}