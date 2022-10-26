using System;
using UnityEngine;

namespace Gameplay
{
public class ProjectileBehaviour: SpaceBehaviour
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
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.TryGetComponent(out SpaceshipBehaviour spaceship))
            _onCollision?.Invoke();
    }
}
}