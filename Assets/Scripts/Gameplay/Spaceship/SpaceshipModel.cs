using UnityEngine;

public class SpaceshipModel
{
    private readonly HealthModel _healthModel;
    private readonly AccelerationModel _accelerationModel;
    
    public Texture2D Texture { get; }

    public SpaceshipModel(HealthModel healthModel, 
        AccelerationModel accelerationModel,
        Texture2D texture)
    {
        _healthModel = healthModel;
        _accelerationModel = accelerationModel;
        
        Texture = texture;
    }
    
    public void DecreaseHealth(int damage) => 
        _healthModel.DecreaseHealth(damage);
    
    public void Accelerate(float deltaTime, out float speed) => 
        _accelerationModel.Accelerate(deltaTime, out speed);

    public void Decelerate(float deltaTime, out float speed) => 
        _accelerationModel.Decelerate(deltaTime, out speed);
}