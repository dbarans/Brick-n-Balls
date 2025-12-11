using UnityEngine;

public interface IInputProvider
{
    Vector2 GetAimInput();
    bool GetFireInput();
    void ResetFireRequest();
    void Enable();
    void Disable();
}

