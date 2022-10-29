using System;
using System.Collections.Generic;
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
    
    private readonly List<ProjectileModel> _activeModels;
    private readonly List<ProjectileBehaviour> _behaviours;
    
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
        _activeModels = new List<ProjectileModel>();
        _behaviours = new List<ProjectileBehaviour>();
        _disposablesContainer = new DisposablesContainer();
    }

    public void Initialize()
    {
        _signalBus.Subscribe<SetSpaceshipDataSignal>(SetData);
        _signalBus.Subscribe<LevelEndedSignal>(OnLevelEnd);
    }
    
    public void Dispose()
    {
        _model?.Dispose();
        _signalBus.Unsubscribe<SetSpaceshipDataSignal>(SetData);
        _signalBus.Unsubscribe<LevelEndedSignal>(OnLevelEnd);
        _disposablesContainer.Dispose();
        _projectilesPool.Clear();
    }
    
    private void SetData(SetSpaceshipDataSignal signal)
    {
        _model?.Dispose();
        _model = GetBlasterModel(signal.Data);
        _behaviours.ForEach(b => b.SetDamage(_model.Damage));
    }
    
    public void TryToFire()
    {
        if (_model.CanFire())
        {
            var projectile = _projectilesPool.Get();
            projectile
                .SetPosition(_spaceshipController.GetBarrelPosition())
                .SetRotation(GetRotation(projectile));
            projectile.Reset();
        }
    }
    
    private void OnLevelEnd()
    {
        while (_activeModels.Count > 0)
            _activeModels[0].Deactivate();
    }
    
    private Quaternion GetRotation(ProjectileModel model)
    {
        return _spaceshipController.GetRotation() * model.InitialRotation;
    }
    
    private BlasterModel GetBlasterModel(SpaceshipData data) 
        => new BlasterModel(data.Damage, data.FireRate, data.ProjectileSpeed);
    
    private ProjectileModel GetProjectileModel()
    {
        var uniformSpeedProvider = new UniformSpeedProvider(_model.ProjectileSpeed);
        var model = new ProjectileModel(uniformSpeedProvider);
        
        return model;
    }
    
    private IPool<ProjectileModel> GetProjectilesPool()
    {
        var projectilesFactory = 
            new Factory<ProjectileModel>(GetProjectileModel);
        var dProjectilesFactory = 
            new DecoratedFactory<ProjectileModel>(projectilesFactory)
                .SetOnCreated(CreateProjectileBehaviour);
        
        return new ProjectilesPool(dProjectilesFactory);
    }
    
    private void CreateProjectileBehaviour(ProjectileModel model)
    {
        var behaviour = _projectileBehavioursFactory.Get();
        model.InitialRotation = behaviour.Rotation;
        
        behaviour
            .SetOnCollision(model.Deactivate)
            .SetDamage(_model.Damage);
        
        _disposablesContainer.Add(model.IsActive
            .Subscribe(isActive =>
            {
                behaviour.SetActive(isActive);
            
                if (isActive)
                {
                    _activeModels.Add(model);
                    model.UpdateSpeed(-behaviour.Forward);
                }
                else
                {
                    _activeModels.Remove(model);
                    _projectilesPool.Return(model);
                }
            }));
        
        _disposablesContainer.Add(model.Position
            .Subscribe(position =>
                behaviour.Position = position));
        
        _disposablesContainer.Add(model.Rotation
            .Subscribe(rotation => 
                behaviour.Rotation = rotation));
        
        _disposablesContainer.Add(model.Speed
            .Subscribe(speed => 
                behaviour.SetSpeed(speed)));
        
        _behaviours.Add(behaviour);
    }
}
}