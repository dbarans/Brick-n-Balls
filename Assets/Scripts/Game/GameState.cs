using Unity.Entities;
using Unity.Mathematics;

public struct GameState : IComponentData
{
    public Entity BallPrefab;
    public int RemainingBalls;
    public float3 BallStartPosition;
    public float BallVelocity;
}

