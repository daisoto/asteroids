using System;
using UniRx;
using UnityEngine;

public class BlasterController: IDisposable
{
    private readonly BlasterModel _model;
    private readonly IPool<ProjectileModel> _projectilesPool;
    private readonly IFactory<ProjectileBehaviour> _projectileBehavioursFactory;
    private readonly Func<Vector3> _barrelPositionProvider;
    
    private readonly DisposablesContainer _disposablesContainer;
    
    public BlasterController(BlasterModel model, Func<Vector3> barrelPositionProvider,
        IFactory<ProjectileBehaviour> projectileBehavioursFactory)
    {
        _model = model;
        _barrelPositionProvider = barrelPositionProvider;
        _projectileBehavioursFactory = projectileBehavioursFactory;
        
        _projectilesPool = GetProjectilesPool();
        _disposablesContainer = new DisposablesContainer();
    }
    
    public void Dispose() => _disposablesContainer.Dispose();
    
    public void TryToFire()
    {
        if (_model.CanFire())
        {
            var projectile = _projectilesPool.Get();
            projectile.SetPosition
                .Execute(_barrelPositionProvider.Invoke());
            projectile.Initialize();
        }
    }
    
    private ProjectileModel GetProjectileModel()
    {
        var uniformSpeedProvider = new UniformSpeedProvider(_model.Speed);
        
        return new ProjectileModel(uniformSpeedProvider);
    }
    
    private IPool<ProjectileModel> GetProjectilesPool()
    {
        return new ProjectilesPool(
            new Factory<ProjectileModel>(GetProjectileModel)
                .SetOnCreated(CreateProjectileBehaviour));
    }
    
    private void CreateProjectileBehaviour(ProjectileModel model)
    {
        var behaviour = _projectileBehavioursFactory.Get();
        
        behaviour
            .SetOnCollision(model.Deactivate)
            .SetDamage(_model.Damage);
        
        _disposablesContainer.Add(model.IsActive
            .Subscribe(isActive =>
            {
                behaviour.SetActive(isActive);
            
                if (!isActive)
                    _projectilesPool.Return(model);
            }));
        
        _disposablesContainer.Add(model.SetPosition
            .Subscribe(position =>
            {
                behaviour.Position = position;
            }));
        
        _disposablesContainer.Add(model.Speed
            .Subscribe(speed =>
            {
                behaviour.SetSpeed(Vector3.forward * speed);
            }));
    }
}