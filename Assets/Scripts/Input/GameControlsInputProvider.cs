using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{

    public class GameControlsInputProvider : IInputProvider
{
    private GameControls _controls;
    private bool _fireRequest; // Flag to track fire input, reset each frame

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

    /// <summary>
    /// Resets fire request flag. Should be called each frame after processing input.
    /// </summary>
    public void ResetFireRequest()
    {
        _fireRequest = false;
    }
}
}

