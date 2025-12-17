using Unity.Entities;
using Unity.Mathematics;

/// <summary>
/// Singleton component storing global game state.
/// Tracks ball prefab, remaining balls count, spawn position, ball velocity, and game start status.
/// </summary>
public struct GameState : IComponentData
{
    public Entity BallPrefab;
    public int RemainingBalls;
    public float3 BallStartPosition;
    public float BallVelocity;
    public bool IsGameStarted;
}

