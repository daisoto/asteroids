using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
public class AsteroidBehaviour: SpaceBehaviour
{
    public int Damage { get; private set;}
    
    [SerializeField]
    private ParticleSystem _explosion;
    
    private Action<int> _onDamage;

    private void OnTriggerEnter(Collider other)
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
    
    public async UniTask ToggleExplosion()
    {
        _explosion.Play();
        
        await UniTask.WaitUntil(() => !_explosion.isPlaying);
    }
}
}