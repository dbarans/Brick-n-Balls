using UnityEngine;
using UnityEngine.InputSystem;

public class GameControlsInputProvider : IInputProvider
{
    private GameControls _controls;
    private bool _fireRequest;

    public GameControlsInputProvider()
    {
        _controls = new GameControls();
    }

    public Vector2 GetAimInput()
    {
        return _controls.Gameplay.Aim.ReadValue<Vector2>();
    }

    public bool GetFireInput()
    {
        return _fireRequest;
    }

    public void Enable()
    {
        _controls.Enable();
        _controls.Gameplay.Fire.performed += context => _fireRequest = true;
    }

    public void Disable()
    {
        _controls.Gameplay.Fire.performed -= context => _fireRequest = false;
        _controls.Disable();
    }

    public void ResetFireRequest()
    {
        _fireRequest = false;
    }
}

