using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
public class AsteroidBehaviour: SpaceBehaviour
{
    [SerializeField]
    private Exploder _exploder;
    
    public int Damage { get; private set;}
    
    private Action<int> _onDamage;
    private Action _onCollide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out ProjectileBehaviour projectile))
            _onDamage?.Invoke(projectile.Damage);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out SpaceshipBehaviour spaceship))
            _onCollide?.Invoke();
    }
    
    public AsteroidBehaviour SetOnCollide(Action onCollide)
    {
        _onCollide = onCollide;
        
        return this;
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
    
    public async UniTask ToggleExplosion() => 
        await _exploder.ToggleExplosion();
}
}