using System;
using UnityEngine;

public class SpaceshipModel
{
    public event Action OnDeath = () => { }; 
    public int Damage { get; } 
    public int FireRate { get; }
    public int MaxHealth { get; }
    public int Health { get; private set; }
    
    private static int _minSpeed = 0;
    
    private readonly int _maxSpeed;
    
    private readonly int _acceleration;
    private readonly int _deceleration;

    private float _speed;
    
    public SpaceshipModel(int damage, int fireRate, int maxSpeed, 
        int maxHealth, int acceleration, int deceleration)
    {
        Damage = damage;
        FireRate = fireRate;
        
        MaxHealth = maxHealth;
        Health = maxHealth;

        _maxSpeed = maxSpeed;
        _acceleration = acceleration;
        _deceleration = deceleration;
    }
    
    public void DecreaseHealth(int damage)
    {
        if (damage <= Health)
        {
            Health = 0;
            OnDeath?.Invoke();
        }
        else 
            Health -= damage;
    }
    
    public void Accelerate(float deltaTime, out float speed)
    {
        _speed = Mathf.Lerp(_speed, _maxSpeed, _acceleration * deltaTime);
        
        speed = _speed;
    }
    
    public void Decelerate(float deltaTime, out float speed)
    {
        _speed = Mathf.Lerp(_minSpeed, _speed, _deceleration * deltaTime);
        
        speed = _speed;
    }
}