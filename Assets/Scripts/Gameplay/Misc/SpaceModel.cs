using UniRx;
using UnityEngine;

namespace Gameplay
{
public class SpaceModel
{
    private readonly ReactiveProperty<bool> _isActive;
    public IReadOnlyReactiveProperty<bool> IsActive => _isActive;
    
    private readonly ReactiveProperty<Vector3> _position;
    public IReadOnlyReactiveProperty<Vector3> Position => _position;
    
    private readonly ReactiveProperty<Quaternion> _rotation;
    public IReadOnlyReactiveProperty<Quaternion> Rotation => _rotation;
    
    public SpaceModel()
    {
        _isActive = new ReactiveProperty<bool>();
        _position = new ReactiveProperty<Vector3>();
        _rotation = new ReactiveProperty<Quaternion>();
    }
    
    public SpaceModel SetPosition(Vector3 position)
    {
        _position.SetValueAndForceNotify(position);
        
        return this;
    }
    
    public SpaceModel SetRotation(Quaternion rotation)
    {
        _rotation.SetValueAndForceNotify(rotation);
        
        return this;
    }
    
    public void Activate() => _isActive.SetValueAndForceNotify(true);
    
    public void Deactivate() => _isActive.SetValueAndForceNotify(false);
}
}