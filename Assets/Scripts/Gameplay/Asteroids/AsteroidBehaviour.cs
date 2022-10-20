using System;
using UnityEngine;

public class AsteroidBehaviour: MonoBehaviour
{
    public int Damage { get; private set;}
    
    public Vector3 Position
    {
        get => transform.position;
        set => transform.position = value;
    }
    
    [SerializeField]
    private Rigidbody _rigidbody;
    
    private Action<int> _onDamage;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Projectile projectile))
            _onDamage?.Invoke(projectile.Damage);
    }
    
    public void SetActive(bool flag) => gameObject.SetActive(flag);
    
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
    
    public AsteroidBehaviour SetDamage(int damage)
    {
        Damage = damage;
        
        return this;
    }
}