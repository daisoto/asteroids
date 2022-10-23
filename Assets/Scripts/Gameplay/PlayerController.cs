using System;
using UniRx;
using UnityEngine;
using Core;

namespace Gameplay
{
public class PlayerController: IDisposable 
{
    private readonly InputManager _inputManager;
    private readonly SpaceshipController _spaceshipController;
    private readonly BlasterController _blasterController;
    
    private IDisposable _observation;
    
    private Vector2 _moving => _inputManager.Move;
    private Vector2 _rotating => _inputManager.Look;
    private bool _isFiring => _inputManager.IsFiring;

    public PlayerController(InputManager inputManager, 
        SpaceshipController spaceshipController,
        BlasterController blasterController)
    {
        _inputManager = inputManager;
        _spaceshipController = spaceshipController;
        _blasterController = blasterController;
    }
    
    public void Dispose()
    {
        _observation?.Dispose();
    }
    
    public void SetActive(bool flag)
    {
        _inputManager.SetActive(flag);
        
        if (flag)
            SetObservation();
        else
            _observation?.Dispose();
    }
    
    private void SetObservation()
    {
        _observation = Observable.EveryUpdate().Subscribe(_ =>
        {
            _spaceshipController.Move(_moving);
            _spaceshipController.Rotate(_rotating);
            if (_isFiring)
                _blasterController.TryToFire();
        });
    }
}
}