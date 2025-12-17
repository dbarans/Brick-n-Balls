using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Ball
{
    /// <summary>
    /// Manages ball physics during flight - maintains constant velocity and locks X position.
    /// </summary>
    [BurstCompile]
    partial struct BallPhysicsSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<BallComponent> ball, RefRW<PhysicsVelocity> physicsVelocity, RefRW<LocalTransform> localTransform)
            in SystemAPI.Query<RefRW<BallComponent>, RefRW<PhysicsVelocity>, RefRW<LocalTransform>>())
        {
            // Keep ball stationary until fired
            if (!ball.ValueRO.IsFired)
            {
                physicsVelocity.ValueRW.Linear = float3.zero;
                physicsVelocity.ValueRW.Angular = float3.zero;
                continue;
            }

            if (!ball.ValueRO.IsInitialized) continue;

            // Maintain constant velocity and lock X axis movement
            float3 currentVel = physicsVelocity.ValueRO.Linear;
            currentVel.x = 0;

            if (math.lengthsq(currentVel) > 0.001f)
            {
                float3 direction = math.normalize(currentVel);
                physicsVelocity.ValueRW.Linear = direction * ball.ValueRO.Velocity;
                localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, math.up());
            }
            else
            {
                // Fallback direction if velocity is too small
                physicsVelocity.ValueRW.Linear = new float3(0, 0, 1) * ball.ValueRO.Velocity;
            }
            physicsVelocity.ValueRW.Angular = float3.zero;

            // Constrain ball position to YZ plane (2D gameplay)
            if (math.abs(localTransform.ValueRO.Position.x) > 0.001f)
            {
                localTransform.ValueRW.Position.x = 0f;
            }
        }
    }
}
}

