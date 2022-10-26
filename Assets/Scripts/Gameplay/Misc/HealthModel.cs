using UniRx;
using System;

namespace Gameplay
{
public class HealthModel
{
    private readonly ReactiveProperty<int> _health;
    public IReadOnlyReactiveProperty<int> Health => _health;
    
    private readonly int _maxHealth;
    
    private Action _onDeath;
    
    public HealthModel(int maxHealth)
    {
        _maxHealth = maxHealth;
        _health = new ReactiveProperty<int>(maxHealth);
    }
    
    public HealthModel SetOnDeath(Action onDeath)
    {
        _onDeath = onDeath;
        
        return this;
    }
    
    public void Restore() => _health.Value = _maxHealth;
    
    public void DecreaseHealth(int damage)
    {
        if (damage >= _health.Value)
        {
            _health.Value = 0;
            _onDeath?.Invoke();
        }
        else 
            _health.Value -= damage;
    }
}
}