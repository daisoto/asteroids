using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gameplay
{
public class SpaceshipBehaviour: SpaceBehaviour
{
    [SerializeField]
    private Renderer _renderer;
    
    [SerializeField]
    private Transform _barrel;
    
    [SerializeField]
    private GameObject _trail;
    
    [SerializeField]
    private Exploder _exploder;
    
    private Action<int> _onDamage;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out AsteroidBehaviour asteroid))
        {
            _onDamage?.Invoke(asteroid.Damage);
            AddImpulse(asteroid.Impulse - Impulse);
        }
    }
    
    [ContextMenu("test")]
    private void test() => AddImpulse(Vector3.back);
    
    public SpaceshipBehaviour SetTexture(Texture2D texture)
    {
        _renderer.material.mainTexture = texture;
        
        return this;
    }
    
    public SpaceshipBehaviour SetOnDamage(Action<int> onDamage)
    {
        _onDamage = onDamage;
        
        return this;
    }

    public void SetTrail(bool flag) => 
        _trail.SetActive(flag);
    
    public Vector3 GetBarrelPosition() => _barrel.position;
    
    public async UniTask ToggleExplosion() => 
        await _exploder.ToggleExplosion();
}
}