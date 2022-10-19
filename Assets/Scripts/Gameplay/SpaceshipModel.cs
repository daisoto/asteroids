public class SpaceshipModel
{
    private readonly HealthModel _healthModel;
    private readonly AccelerationModel _accelerationModel;
    private readonly BlasterModel _blasterModel;
    
    public SpaceshipModel(HealthModel healthModel, 
        AccelerationModel accelerationModel, 
        BlasterModel blasterModel)
    {
        _healthModel = healthModel;
        _accelerationModel = accelerationModel;
        _blasterModel = blasterModel;
    }
    
    public void DecreaseHealth(int damage) => 
        _healthModel.DecreaseHealth(damage);
    
    public void Accelerate(float deltaTime, out float speed) => 
        _accelerationModel.Accelerate(deltaTime, out speed);

    public void Decelerate(float deltaTime, out float speed) => 
        _accelerationModel.Decelerate(deltaTime, out speed);
    
    public bool TryToFire(out int damage) => 
        _blasterModel.TryToFire(out damage);
}