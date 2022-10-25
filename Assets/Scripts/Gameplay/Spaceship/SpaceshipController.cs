using UniRx;
using System;
using Data;
using UnityEngine;
using Zenject;

namespace Gameplay
{
public class SpaceshipController: 
    IInitializable, IDisposable, IScreenDepthProvider
{
    private readonly SpaceshipBehaviour _behaviour;
    private readonly SignalBus _signalBus;
    private readonly SpaceshipDataManager _spaceshipDataManager;
    private readonly Camera _camera;
    private readonly ITextureProvider _textureProvider;

    private SpaceshipModel _model;
    
    public IReadOnlyReactiveProperty<int> Health => _health;
    private readonly ReactiveProperty<int> _health;
    public IReadOnlyReactiveProperty<int> MaxHealth => _maxHealth;
    private readonly ReactiveProperty<int> _maxHealth;
    
    private IDisposable _healthSubscription;

    public SpaceshipController(SpaceshipBehaviour behaviour, 
        SignalBus signalBus, SpaceshipDataManager spaceshipDataManager, 
        ITextureProvider textureProvider, Camera camera)
    {
        _behaviour = behaviour;
        _signalBus = signalBus;
        _spaceshipDataManager = spaceshipDataManager;
        _textureProvider = textureProvider;
        _camera = camera;
        
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
    
    public float GetDepth() => 
        _camera.transform.position.y - _behaviour.Position.y;
    
    private void SetData(SetSpaceshipDataSignal signal)
    {
        var data = signal.Data;
        
        if (signal.IsNew)
            _spaceshipDataManager.Save(data);
        
        _maxHealth.Value = data.MaxHealth;
        
        _model = GetSpaceshipModel(data);
        _model.UpdateSpeed();

        _behaviour.SetTexture(_textureProvider.GetTexture(data.Title));
        
        _healthSubscription?.Dispose();
        _healthSubscription = _model.Health.Subscribe(health =>
        {
            _health.Value = health;
        });
    }
    
    public void Move(Vector2 delta)
    {
        var motion = _model.Speed.Value * delta;
        var speed = new Vector3(motion.x, 0, motion.y);
        _behaviour.SetSpeed(speed);
        _behaviour.SetTrail(speed != Vector3.zero);
    }
    
    public void Rotate(Vector2 position)
    {
        var targetPosition = _camera.ScreenToWorldPoint(
            new Vector3(position.x, position.y, GetDepth()));
        
        var relativePos = _behaviour.Position - targetPosition;
        var rotation = Quaternion.LookRotation(relativePos);
        
        _behaviour.Rotation = rotation;
    }
    
    public Quaternion GetRotation() => _behaviour.Rotation;

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