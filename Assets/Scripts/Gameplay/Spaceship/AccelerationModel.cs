using UnityEngine;

public class AccelerationModel
{
    private static int _minSpeed = 0;
    
    private readonly int _maxSpeed;
    
    private readonly int _acceleration;
    private readonly int _deceleration;

    private float _speed;
    
    public AccelerationModel(int maxSpeed, int acceleration, int deceleration)
    {
        _maxSpeed = maxSpeed;
        _acceleration = acceleration;
        _deceleration = deceleration;
    }
    
    public void Accelerate(float deltaTime, out float speed)
    {
        _speed = Mathf.Lerp(_speed, _maxSpeed, _acceleration * deltaTime);
        
        speed = _speed;
    }
    
    public void Decelerate(float deltaTime, out float speed)
    {
        _speed = Mathf.Lerp(_minSpeed, _speed, _deceleration * deltaTime);
        
        speed = _speed;
    }
}