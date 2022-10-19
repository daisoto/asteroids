public class BlasterController
{
    private readonly BlasterModel _model;
    private readonly BlasterBehaviour _behaviour;
    
    public BlasterController(BlasterModel model, BlasterBehaviour behaviour)
    {
        _model = model;
        _behaviour = behaviour;
    }
    
    public void TryToFire()
    {
        if (_model.CanFire())
            _behaviour.Fire(_model.Damage);
    }
}