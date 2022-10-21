﻿using UniRx;
using System;

public class HealthModel
{
    public int MaxHealth { get; }
    
    private readonly ReactiveProperty<int> _health;
    public IReadOnlyReactiveProperty<int> Health => _health;
    
    private Action _onDeath;
    
    public HealthModel(int maxHealth)
    {
        MaxHealth = maxHealth;
        _health = new ReactiveProperty<int>(maxHealth);
    }
    
    public HealthModel SetOnDeath(Action onDeath)
    {
        _onDeath = onDeath;
        
        return this;
    }
    
    public void DecreaseHealth(int damage)
    {
        if (damage <= _health.Value)
        {
            _health.Value = 0;
            _onDeath?.Invoke();
        }
        else 
            _health.Value -= damage;
    }
}