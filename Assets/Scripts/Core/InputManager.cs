using UnityEngine;
using UnityEngine.InputSystem;

namespace Core 
{
public class InputManager
{
    public bool IsFiring { get; private set; }
    
    public Vector2 Move { get; private set; }
    public Vector2 Look { get; private set; }

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

    private void Subscribe()
    {
        _playerControls.Player.Move.performed += ProcessMoveInput;
        _playerControls.Player.Move.canceled += ProcessMoveInput;
        
        _playerControls.Player.Rotate.performed += ProcessRotateInput;
        _playerControls.Player.Rotate.canceled += ProcessRotateInput;
        
        _playerControls.Player.Fire.performed += ProcessFireInput;
        _playerControls.Player.Fire.canceled += ProcessFireInput;
    }

    private void Unsubscribe()
    {
        _playerControls.Player.Move.performed -= ProcessMoveInput;
        _playerControls.Player.Move.canceled -= ProcessMoveInput;
        
        _playerControls.Player.Rotate.performed -= ProcessRotateInput;
        _playerControls.Player.Rotate.canceled -= ProcessRotateInput;
        
        _playerControls.Player.Fire.performed -= ProcessFireInput;
        _playerControls.Player.Fire.canceled -= ProcessFireInput;
    }

    private void ProcessMoveInput(InputAction.CallbackContext ctx) => 
        Move = ctx.ReadValue<Vector2>();

    private void ProcessRotateInput(InputAction.CallbackContext ctx) => 
        Look = ctx.ReadValue<Vector2>();
    
    private void ProcessFireInput(InputAction.CallbackContext ctx) =>
        IsFiring = ctx.ReadValue<float>().Equals(1);
}
}