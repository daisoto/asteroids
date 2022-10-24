using System;
using Data;
using UnityEngine;
using Zenject;

namespace Gameplay
{
public class SpaceshipController: IInitializable, IDisposable
{
    private readonly SpaceshipBehaviour _behaviour;
    private readonly SignalBus _signalBus;
    private readonly SpaceshipDataManager _spaceshipDataManager;

    private SpaceshipModel _model;
    
    public SpaceshipController(SpaceshipBehaviour behaviour, 
        SignalBus signalBus, SpaceshipDataManager spaceshipDataManager)
    {
        _behaviour = behaviour;
        _signalBus = signalBus;
        _spaceshipDataManager = spaceshipDataManager;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<SetSpaceshipDataSignal>(SetData);
        
        _behaviour
            .SetOnDamage(ReceiveDamage);
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<SetSpaceshipDataSignal>(SetData);
    }
    
    private void SetData(SetSpaceshipDataSignal signal)
    {
        var data = signal.Data;
        
        if (signal.IsNew)
            _spaceshipDataManager.Save(data);
        
        _model = GetSpaceshipModel(data);
        _behaviour.SetTexture(_model.Texture);
    }
    
    public void Move(Vector2 delta)
    {
        var motion = _model.Speed * delta;
        _behaviour.Move(motion);
    }
    
    public void Rotate(Vector2 position) => _behaviour.Rotate(position);
    
    public Vector3 GetBarrelPosition() =>  _behaviour.GetBarrelPosition();
    
    private void ReceiveDamage(int damage) => _model.DecreaseHealth(damage); 
    
    private SpaceshipModel GetSpaceshipModel(SpaceshipData data)
    {
        var healthModel = new HealthModel(data.MaxHealth);
        var speedProvider = new UniformSpeedProvider(data.Speed);
        
        return new SpaceshipModel(
            healthModel, speedProvider, data.Texture);
    }
}
}