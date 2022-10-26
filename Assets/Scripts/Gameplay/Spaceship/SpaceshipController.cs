using UniRx;
using System;
using Cysharp.Threading.Tasks;
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
    private readonly IWorldPointProvider _worldPointProvider;
    
    public IReadOnlyReactiveProperty<int> Health => _health;
    private readonly ReactiveProperty<int> _health;
    public IReadOnlyReactiveProperty<int> MaxHealth => _maxHealth;
    private readonly ReactiveProperty<int> _maxHealth;
    
    private readonly DisposablesContainer _disposablesContainer;
    
    private readonly Vector3 _initialPosition;
    private readonly Quaternion _initialRotation;
    
    private SpaceshipModel _model;
    private Action _onExplosion;

    public SpaceshipController(SpaceshipBehaviour behaviour, 
        SignalBus signalBus, SpaceshipDataManager spaceshipDataManager, 
        ITextureProvider textureProvider,
        IWorldPointProvider worldPointProvider)
    {
        _behaviour = behaviour;
        _signalBus = signalBus;
        _spaceshipDataManager = spaceshipDataManager;
        _textureProvider = textureProvider;
        _worldPointProvider = worldPointProvider;
        
        _initialPosition = behaviour.Position;
        _initialRotation = behaviour.Rotation;

        _health = new ReactiveProperty<int>();
        _maxHealth = new ReactiveProperty<int>();
        _disposablesContainer = new DisposablesContainer();
    }

    public void Initialize()
    {        
        _signalBus.Subscribe<LevelStartedSignal>(Activate);
        _signalBus.Subscribe<SetSpaceshipDataSignal>(SetData);
        
        _behaviour
            .SetOnDamage(ReceiveDamage);
    }
    
    public void Dispose()
    {
        _signalBus.Unsubscribe<LevelStartedSignal>(Activate);
        _signalBus.Unsubscribe<SetSpaceshipDataSignal>(SetData);
        _disposablesContainer.Dispose();
    }
    
    public SpaceshipController SetOnExplosion(Action onExplosion)
    {
        _onExplosion = onExplosion;
        
        return this;
    }
    
    public void Move(Vector2 delta)
    {
        var motion = new Vector3(delta.x, 0, delta.y);
        _model.UpdateSpeed(motion);
    }
    
    public void Rotate(Vector2 position)
    {
        var targetPosition = _worldPointProvider.GetFromScreen(position);
        var relativePos = _model.Position.Value - targetPosition;
        var rotation = Quaternion.LookRotation(relativePos);
        
        _model.SetRotation(rotation);
    }
    
    public Quaternion GetRotation() => _model.Rotation.Value;

    public Vector3 GetBarrelPosition() =>  _behaviour.GetBarrelPosition();
    
    private void ReceiveDamage(int damage) => _model.DecreaseHealth(damage); 
    
    private void SetData(SetSpaceshipDataSignal signal)
    {
        var data = signal.Data;
        
        if (signal.IsNew)
            _spaceshipDataManager.Save(data);
        
        _maxHealth.Value = data.MaxHealth;
        
        _model = GetSpaceshipModel(data);

        _behaviour.SetTexture(_textureProvider.GetTexture(data.Title));
        
        _disposablesContainer.Dispose();
        
        _disposablesContainer.Add(_model.Health
            .Subscribe(health => _health.Value = health));
        _disposablesContainer.Add(_model.Position
            .Subscribe(pos => _behaviour.Position = pos));
        _disposablesContainer.Add(_model.Rotation
            .Subscribe(rot => _behaviour.Rotation = rot));
        _disposablesContainer.Add(_model.Speed
            .Subscribe(speed =>
            {
                _behaviour.SetTrail(speed != Vector3.zero);
                _behaviour.SetSpeed(speed);
            }));
    }
    
    private SpaceshipModel GetSpaceshipModel(SpaceshipData data)
    {
        var healthModel = new HealthModel(data.MaxHealth);
        var speedProvider = new UniformSpeedProvider(data.Speed);
        healthModel.SetOnDeath(Explode);
        
        return new SpaceshipModel(healthModel, speedProvider);
    }
    
    private void Activate()
    {
        _model.Restore()
            .SetPosition(_initialPosition)
            .SetRotation(_initialRotation);
        
        _behaviour.SetBaseModel(true);
        _behaviour.SetActive(true);
    }
    
    private void Explode() =>
        ExplodeInternal().Forget();
    
    private async UniTask ExplodeInternal()
    {
        _onExplosion?.Invoke();
        _behaviour.SetBaseModel(false);
        await _behaviour.ToggleExplosion();
        _behaviour.SetActive(false);
        _signalBus.Fire(new SpaceshipDestroyedSignal());
    }
}
}