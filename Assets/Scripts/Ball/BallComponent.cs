using Unity.Entities;
using Unity.Mathematics;

namespace Ball
{
    /// <summary>
    /// ECS component data for ball entities.
    /// Stores velocity, firing direction, and initialization state.
    /// </summary>
    public struct BallComponent : IComponentData
{
    public float Velocity;
    public float3 InitialDirection;
    public bool IsInitialized;
    public bool IsFired;
}
}