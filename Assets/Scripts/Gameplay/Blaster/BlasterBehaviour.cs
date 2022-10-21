using UnityEngine;
using Zenject;

public class BlasterBehaviour: MonoBehaviour
{
    [Inject]
    private Projectile.Pool _pool;
    
    public void Fire(int damage)
    {
        var projectile = _pool.Spawn();
        projectile
            .AddForce(Vector3.forward)
            .SetOnCollision(Despawn)
            .Damage = damage;
            
        void Despawn() => _pool.Despawn(projectile);
    }
}