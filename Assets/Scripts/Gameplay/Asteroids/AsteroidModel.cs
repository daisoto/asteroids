using UniRx;
using UnityEngine;
using Zenject;

public class AsteroidModel: IInitializable
{
    public int Damage { get; }
    
    private readonly ReactiveProperty<bool> _isAlive;
    public IReadOnlyReactiveProperty<bool> IsAlive => _isAlive;
    
    private readonly ReactiveProperty<float> _speed;
    public IReadOnlyReactiveProperty<float> Speed => _speed;
    
    public readonly ReactiveCommand<Vector2> SetPosition;

    private readonly HealthModel _healthModel;
    private readonly float _maxSpeed;
    private readonly float _minSpeed;
    
    public AsteroidModel(HealthModel healthModel, int damage, float maxSpeed, float minSpeed)
    {
        _healthModel = healthModel;
        Damage = damage;
        
        _maxSpeed = maxSpeed;
        _minSpeed = minSpeed;

        SetPosition = new ReactiveCommand<Vector2>();
        _isAlive = new ReactiveProperty<bool>();
        _speed = new ReactiveProperty<float>();
        
        _healthModel.SetOnDeath(SetDead);
    }

    public void Initialize()
    {
        _isAlive.Value = true;
        _speed.Value = RandomUtility.GetFloat(_minSpeed, _maxSpeed);
    }
    
    public void DecreaseHealth(int damage) => _healthModel.DecreaseHealth(damage);
    
    private void SetDead()
    {
        _isAlive.Value = false;
    }
}