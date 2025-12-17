using System;

/// <summary>
/// Central communication bridge between ECS systems and MonoBehaviour UI components.
/// Provides static events for cross-layer communication.
/// </summary>
public static class GameEventBridge
{
    /// <summary>
    /// Event fired when game over condition is detected. Parameter: final score.
    /// </summary>
    public static Action<int> OnGameOver;

    /// <summary>
    /// Resets all events. Useful when reloading scenes to prevent memory leaks.
    /// </summary>
    public static void ResetEvents()
    {
        OnGameOver = null;
    }
}
