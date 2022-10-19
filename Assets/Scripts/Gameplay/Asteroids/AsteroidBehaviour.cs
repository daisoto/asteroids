using System;
using UnityEngine;

public class AsteroidBehaviour: MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    
    private Action<int> _onDamage;
    
    public int Damage { get; set; } 

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Projectile projectile))
            _onDamage?.Invoke(projectile.Damage);
    }
    
    public AsteroidBehaviour SetOnDamage(Action<int> onDamage)
    {
        _onDamage = onDamage;
        
        return this;
    }
    
    public AsteroidBehaviour SetSpeed(Vector2 speed)
    {
        _rigidbody.velocity = speed;
        
        return this;
    }
}