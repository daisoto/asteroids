using UniRx;
using UnityEngine;
using Zenject;

public class ProjectileModel: IInitializable
{
    private readonly PositionableModel _positionableModel;
    private readonly ISpeedProvider _speedProvider;
    
    public IReadOnlyReactiveProperty<float> Speed => _speedProvider.Speed;
    public ReactiveCommand<Vector2> SetPosition => _positionableModel.SetPosition; 
    public IReadOnlyReactiveProperty<bool> IsActive => _positionableModel.IsActive;
    
    public ProjectileModel(ISpeedProvider speedProvider)
    {
        _speedProvider = speedProvider;
        
        _positionableModel = new PositionableModel();
    }
    
    public void Initialize()
    {
        _positionableModel.Initialize();
        _speedProvider.Initialize();
    }
    
    public void Deactivate() => _positionableModel.Deactivate();
}