using UnityEngine;
using Zenject;

public class SpaceshipController: IInitializable
{
    private readonly SpaceshipModel _model;
    private readonly SpaceshipBehaviour _behaviour;
    
    public SpaceshipController(SpaceshipModel model, SpaceshipBehaviour behaviour)
    {
        _model = model;
        _behaviour = behaviour;
    }

    public void Initialize()
    {
        _behaviour
            .SetOnDamage(ReceiveDamage)
            .SetTexture(_model.Texture);
    }
    
    public void Move(Vector2 delta)
    {
        var motion = _model.Speed * delta;
        _behaviour.Move(motion);
    }
    
    public void Rotate(Vector2 position) => _behaviour.Rotate(position);
    
    public Vector3 GetBarrelPosition() =>  _behaviour.GetBarrelPosition();
    
    private void ReceiveDamage(int damage) => _model.DecreaseHealth(damage); 
}