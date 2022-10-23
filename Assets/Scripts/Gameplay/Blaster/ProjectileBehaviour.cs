using System;
using UnityEngine;

namespace Gameplay
{
public class ProjectileBehaviour: PositionableBehaviour
{
    private Action _onCollision;
    
    public int Damage { get; private set; }
    
    public ProjectileBehaviour SetDamage(int damage)
    {
        Damage = damage;
        
        return this;
    }
    
    public ProjectileBehaviour SetOnCollision(Action onCollision)
    {
        _onCollision = onCollision;
        
        return this;
    }
    
    private void OnCollisionEnter(Collision other) // todo стены
    {
        if (other.gameObject.TryGetComponent(out AsteroidBehaviour asteroid))
            _onCollision?.Invoke();
    }
}
}