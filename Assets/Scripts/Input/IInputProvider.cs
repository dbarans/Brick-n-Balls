using UnityEngine;

namespace Input
{
    /// <summary>
    /// Abstraction for player input handling (aim and fire).
    /// </summary>
    public interface IInputProvider
    {
        Vector2 GetAimInput();
        bool GetFireInput();
        void ResetFireRequest();
        void Enable();
        void Disable();
    }
}

