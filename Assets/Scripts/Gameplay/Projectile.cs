using System;
using UnityEngine;
using Zenject;

public class Projectile: MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidbody;
    
    private Action _onCollision;
    
    public int Damage { get; set; }
    
    public Projectile AddForce(Vector3 force)
    {
        _rigidbody.AddForce(force);
        
        return this;
    }
    
    public Projectile SetOnCollision(Action onCollision)
    {
        _onCollision = onCollision;
        
        return this;
    }
    
    private void OnCollisionEnter(Collision other) // todo стены
    {
        if (other.gameObject.TryGetComponent(out AsteroidBehaviour asteroid))
            _onCollision?.Invoke();
    }
    
    public class Pool : MonoMemoryPool<Projectile> { }
}