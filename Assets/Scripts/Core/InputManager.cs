using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core 
{
public class InputManager: IResettable
{
    public bool IsFiring { get; private set; }
    
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }
    
    public event Action OnPause = () => {}; 

    private readonly PlayerControls _playerControls;

    public InputManager()
    {
        _playerControls = new PlayerControls();
    }

    public void SetActive(bool state)
    {
        if (state)
        {
            Subscribe();
            _playerControls.Enable();
        }
        else
        {
            Unsubscribe();
            _playerControls.Disable();
        }
    }
    
    public void Reset()
    {
        Move = Vector2.zero;
        Look = Vector2.zero;
        IsFiring = false;
    }

    private void Subscribe()
    {
        _playerControls.Player.Move.performed += ProcessMoveInput;
        _playerControls.Player.Move.canceled += ProcessMoveInput;
        
        _playerControls.Player.Rotate.performed += ProcessRotateInput;
        _playerControls.Player.Rotate.canceled += ProcessRotateInput;
        
        _playerControls.Player.Fire.performed += ProcessFireInput;
        _playerControls.Player.Fire.canceled += ProcessFireInput;
        
        _playerControls.Player.Pause.performed += ProcessPause;
    }

    private void Unsubscribe()
    {
        _playerControls.Player.Move.performed -= ProcessMoveInput;
        _playerControls.Player.Move.canceled -= ProcessMoveInput;
        
        _playerControls.Player.Rotate.performed -= ProcessRotateInput;
        _playerControls.Player.Rotate.canceled -= ProcessRotateInput;
        
        _playerControls.Player.Fire.performed -= ProcessFireInput;
        _playerControls.Player.Fire.canceled -= ProcessFireInput;
        
        _playerControls.Player.Pause.performed -= ProcessPause;
    }

    private void ProcessMoveInput(InputAction.CallbackContext ctx) => 
        Move = ctx.ReadValue<Vector2>();

    private void ProcessRotateInput(InputAction.CallbackContext ctx) => 
        Look = ctx.ReadValue<Vector2>();
    
    private void ProcessFireInput(InputAction.CallbackContext ctx) =>
        IsFiring = ctx.ReadValue<float>().Equals(1);
    
    private void ProcessPause(InputAction.CallbackContext ctx) =>
        OnPause?.Invoke();
}
}