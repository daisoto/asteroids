using System;
using UnityEngine;

namespace Gameplay
{
public class AsteroidBehaviour: PositionableBehaviour
{
    public int Damage { get; private set;}
    
    private Action<int> _onDamage;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out ProjectileBehaviour projectile))
            _onDamage?.Invoke(projectile.Damage);
    }

    public AsteroidBehaviour SetOnDamage(Action<int> onDamage)
    {
        _onDamage = onDamage;
        
        return this;
    }

    public AsteroidBehaviour SetDamage(int damage)
    {
        Damage = damage;
        
        return this;
    }
}
}