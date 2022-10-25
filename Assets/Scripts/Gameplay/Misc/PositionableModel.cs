using UniRx;
using UnityEngine;
using Zenject;

namespace Gameplay
{
public class PositionableModel
{
    private readonly ReactiveProperty<bool> _isActive;
    public IReadOnlyReactiveProperty<bool> IsActive => _isActive;
    
    public readonly ReactiveCommand<Vector2> SetPosition;
    
    public PositionableModel()
    {
        _isActive = new ReactiveProperty<bool>();
        SetPosition = new ReactiveCommand<Vector2>();
    }
    
    public void Activate()
    {
        _isActive.Value = true;
    }
    
    public void Deactivate()
    {
        _isActive.Value = false;
    }
}
}