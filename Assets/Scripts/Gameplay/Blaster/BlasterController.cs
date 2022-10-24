using System;
using Data;
using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay
{
public class BlasterController: IInitializable, IDisposable
{
    private readonly IPool<ProjectileModel> _projectilesPool;
    private readonly IFactory<ProjectileBehaviour> _projectileBehavioursFactory;
    private readonly SpaceshipController _spaceshipController;
    private readonly SignalBus _signalBus; 
    
    private readonly DisposablesContainer _disposablesContainer;
    
    private BlasterModel _model;
    
    public BlasterController(SpaceshipController spaceshipController,
        IFactory<ProjectileBehaviour> projectileBehavioursFactory, 
        SignalBus signalBus)
    {
        _spaceshipController = spaceshipController;
        _projectileBehavioursFactory = projectileBehavioursFactory;
        _signalBus = signalBus;

        _projectilesPool = GetProjectilesPool();
        _disposablesContainer = new DisposablesContainer();
    }

    public void Initialize()
    {
        _signalBus.Subscribe<SetSpaceshipDataSignal>(SetData);
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<SetSpaceshipDataSignal>(SetData);
        _disposablesContainer.Dispose();
    }
    
    private void SetData(SetSpaceshipDataSignal signal)
    {
        _model = GetBlasterModel(signal.Data);
    }
    
    public void TryToFire()
    {
        if (_model.CanFire())
        {
            var projectile = _projectilesPool.Get();
            projectile.SetPosition
                .Execute(_spaceshipController.GetBarrelPosition());
            projectile.Initialize();
        }
    }
    
    private BlasterModel GetBlasterModel(SpaceshipData data) 
        => new BlasterModel(data.Damage, data.FireRate, data.ProjectileSpeed);
    
    private ProjectileModel GetProjectileModel()
    {
        var uniformSpeedProvider = new UniformSpeedProvider(_model.ProjectileSpeed);
        
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
}