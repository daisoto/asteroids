﻿using System;
using UnityEngine;

public class SpaceshipBehaviour: MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    
    [SerializeField]
    private Renderer _renderer;
    
    [SerializeField]
    private CharacterController _characterController;
    
    [SerializeField]
    private Transform _barrel;
    
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
    
    public Vector3 GetBarrelPosition() => _barrel.position;

    public void Rotate(Vector2 position)
    {
        var targetPosition = _camera.ScreenToWorldPoint(position);
        var relativePos = targetPosition - transform.position;
        var rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;
    }
    
    public void Move(Vector3 motion)
    {
        _characterController.Move(motion); 
    }
}