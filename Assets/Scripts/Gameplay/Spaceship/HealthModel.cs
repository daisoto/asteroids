using System;

public class HealthModel
{
    public int MaxHealth { get; }
    public int Health { get; private set; }
    
    private Action _onDeath;
    
    public HealthModel(int maxHealth, Action onDeath)
    {
        MaxHealth = maxHealth;
        Health = maxHealth;
        _onDeath = onDeath;
    }
    
    public void DecreaseHealth(int damage)
    {
        if (damage <= Health)
        {
            Health = 0;
            _onDeath?.Invoke();
        }
        else 
            Health -= damage;
    }
}