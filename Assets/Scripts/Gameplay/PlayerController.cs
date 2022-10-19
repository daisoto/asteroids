using UnityEngine;
using Zenject;

public class PlayerController: IInitializable
{
    private readonly InputManager _inputManager;
    private readonly SpaceshipBehaviour _spaceshipBehaviour;

    public PlayerController(InputManager inputManager, SpaceshipBehaviour spaceshipBehaviour)
    {
        _inputManager = inputManager;
        _spaceshipBehaviour = spaceshipBehaviour;
    }

    public void Initialize()
    {
        _spaceshipBehaviour
            .SetMover(GetMoving)
            .SetRotator(GetRotating)
            .SetFirer(IsFiring);
    }
    
    public void SetActive(bool flag)
    {
        _inputManager.SetActive(flag);
        _spaceshipBehaviour.enabled = flag;
    }
    
    private Vector2 GetMoving() => _inputManager.Move;
    
    private Vector2 GetRotating() => _inputManager.Look;
    
    private bool IsFiring() => _inputManager.IsFiring;
}
