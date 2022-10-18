using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SpaceshipBehaviour: MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    
    [SerializeField]
    private Renderer _renderer;
    
    [SerializeField]
    private CharacterController _characterController;
    
    private SpaceshipModel _spaceshipModel;

    private Func<Vector2> _rotationProvider;
    private Func<Vector2> _movingProvider;
    private Func<bool> _firingProvider;

    private void Update()
    {
        Rotate(_rotationProvider.Invoke());
        Move(_movingProvider.Invoke());
        
        if (_firingProvider.Invoke())
            Fire();
    }

    public SpaceshipBehaviour SetSpaceshipModel(SpaceshipModel spaceshipModel)
    {
        _spaceshipModel = spaceshipModel;
        
        return this;
    }
    
    public SpaceshipBehaviour SetTexture(Texture2D texture)
    {
        _renderer.material.mainTexture = texture;
        
        return this;
    }
    
    public SpaceshipBehaviour SetRotator(Func<Vector2> rotationProvider)
    {
        _rotationProvider = rotationProvider;
        
        return this;
    }
    
    public SpaceshipBehaviour SetMover(Func<Vector2> movingProvider)
    {
        _movingProvider = movingProvider;
        
        return this;
    }
    
    public SpaceshipBehaviour SetFirer(Func<bool> firingProvider)
    {
        _firingProvider = firingProvider;
        
        return this;
    }
    
    private void Rotate(Vector2 position)
    {
        var targetPosition = _camera.ScreenToWorldPoint(position);
        var relativePos = targetPosition - transform.position;
        var rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = rotation;
    }
    
    private void Move(Vector2 delta)
    {
        float speed;
        
        if (delta.Equals(Vector2.zero))
            _spaceshipModel.Decelerate(Time.deltaTime, out speed);
        else
            _spaceshipModel.Accelerate(Time.deltaTime, out speed);
        
        _characterController.Move(delta * speed); 
    }
    
    private void Fire()
    {
        
    }
}