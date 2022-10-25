using System;
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
    
    private Action<int> _onDamage;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out AsteroidBehaviour asteroid))
            _onDamage?.Invoke(asteroid.Damage);
    }
    
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

    public override void SetSpeed(Vector3 speed)
    {
        _trail.SetActive(speed != Vector3.zero);
        base.SetSpeed(speed);
    }

    public Vector3 GetBarrelPosition() => _barrel.position;

    public void Rotate(Quaternion rotation) => transform.rotation = rotation;
    
}
}