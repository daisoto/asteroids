using System;
using UniRx;
using UnityEngine;
using Core;
using UI;
using UnityEngine.EventSystems;
using Zenject;

namespace Gameplay
{
public class PlayerController: IInitializable, IDisposable 
{
    private readonly InputManager _inputManager;
    private readonly SpaceshipController _spaceshipController;
    private readonly InGamePresenter _inGamePresenter;
    private readonly BlasterController _blasterController;
    private readonly EventSystem _eventSystem;
    
    private IDisposable _updateObservation;
    
    private Vector2 _moving => _inputManager.Move;
    private Vector2 _rotating => _inputManager.Look;
    private bool _isFiring => _inputManager.IsFiring;

    public PlayerController(InputManager inputManager, 
        SpaceshipController spaceshipController,
        BlasterController blasterController, 
        InGamePresenter inGamePresenter,
        EventSystem eventSystem)
    {
        _inputManager = inputManager;
        _spaceshipController = spaceshipController;
        _blasterController = blasterController;
        _eventSystem = eventSystem;
        _inGamePresenter = inGamePresenter;
    }
    
    public void Initialize() =>
        _spaceshipController.SetOnExplosion(Deactivate);
    
    public void Dispose() 
    {
        _updateObservation?.Dispose();
        _inputManager.OnPause -= _inGamePresenter.ShowMenu;
    }
    
    public void SetActive(bool flag)
    {
        _inputManager.SetActive(flag);
        
        if (flag)
            SetObservation();
        else
        {
            Dispose();
            _inputManager.Reset();
        }
    }
    
    private void SetObservation()
    {
        _inputManager.OnPause += _inGamePresenter.ShowMenu;
        
        _updateObservation = Observable.EveryUpdate().Subscribe(_ =>
        {
            _spaceshipController.Move(_moving);
            _spaceshipController.Rotate(_rotating);
            if (_isFiring && !_eventSystem.IsPointerOverGameObject())
                _blasterController.TryToFire();
        });
    }
    
    private void Deactivate() => SetActive(false);
}
}