using UniRx;
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
    private readonly ITextureProvider _textureProvider;

    private SpaceshipModel _model;
    
    public IReadOnlyReactiveProperty<int> Health => _health;
    private readonly ReactiveProperty<int> _health;
    public IReadOnlyReactiveProperty<int> MaxHealth => _maxHealth;
    private readonly ReactiveProperty<int> _maxHealth;
    
    private IDisposable _healthSubscription;

    public SpaceshipController(SpaceshipBehaviour behaviour, 
        SignalBus signalBus, SpaceshipDataManager spaceshipDataManager, 
        ITextureProvider textureProvider)
    {
        _behaviour = behaviour;
        _signalBus = signalBus;
        _spaceshipDataManager = spaceshipDataManager;
        _textureProvider = textureProvider;
        _health = new ReactiveProperty<int>();
        _maxHealth = new ReactiveProperty<int>();
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
        _healthSubscription?.Dispose();
    }
    
    private void SetData(SetSpaceshipDataSignal signal)
    {
        var data = signal.Data;
        
        if (signal.IsNew)
            _spaceshipDataManager.Save(data);
        
        _maxHealth.Value = data.MaxHealth;
        _model = GetSpaceshipModel(data);
        _behaviour.SetTexture(_textureProvider.Get(data.Title));
        
        _healthSubscription = _model.Health.Subscribe(health =>
        {
            _health.Value = health;
        });
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
        
        return new SpaceshipModel(healthModel, speedProvider);
    }
}
}