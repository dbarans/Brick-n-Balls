using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

/// <summary>
/// Manages ball physics during flight - maintains constant velocity and locks X position.
/// </summary>
[BurstCompile]
partial struct BallPhysicsSystem : ISystem
{
    private const float VelocityEpsilon = 0.001f;
    private const float PositionEpsilon = 0.001f;
    private static readonly float3 FallbackDirection = new float3(0, 0, 1);

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<BallComponent> ball, RefRW<PhysicsVelocity> physicsVelocity, RefRW<LocalTransform> localTransform)
            in SystemAPI.Query<RefRW<BallComponent>, RefRW<PhysicsVelocity>, RefRW<LocalTransform>>())
        {
            if (!ball.ValueRO.IsFired)
            {
                physicsVelocity.ValueRW.Linear = float3.zero;
                physicsVelocity.ValueRW.Angular = float3.zero;
                continue;
            }

            if (!ball.ValueRO.IsInitialized) continue;

            float3 currentVel = physicsVelocity.ValueRO.Linear;
            currentVel.x = 0;

            if (math.lengthsq(currentVel) > VelocityEpsilon)
            {
                float3 direction = math.normalize(currentVel);
                physicsVelocity.ValueRW.Linear = direction * ball.ValueRO.Velocity;
                localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, math.up());
            }
            else
            {
                physicsVelocity.ValueRW.Linear = FallbackDirection * ball.ValueRO.Velocity;
            }
            physicsVelocity.ValueRW.Angular = float3.zero;

            if (math.abs(localTransform.ValueRO.Position.x) > PositionEpsilon)
            {
                localTransform.ValueRW.Position.x = 0f;
            }
        }
    }
}

