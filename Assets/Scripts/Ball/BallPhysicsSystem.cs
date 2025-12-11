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
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<BallComponent> ball, RefRW<PhysicsVelocity> physicsVelocity, RefRW<LocalTransform> localTransform)
            in SystemAPI.Query<RefRW<BallComponent>, RefRW<PhysicsVelocity>, RefRW<LocalTransform>>())
        {
            if (!ball.ValueRO.isFired)
            {
                physicsVelocity.ValueRW.Linear = float3.zero;
                physicsVelocity.ValueRW.Angular = float3.zero;
                continue;
            }

            if (!ball.ValueRO.isInitialized) continue;

            float3 currentVel = physicsVelocity.ValueRO.Linear;
            currentVel.x = 0;

            if (math.lengthsq(currentVel) > 0.001f)
            {
                float3 direction = math.normalize(currentVel);
                physicsVelocity.ValueRW.Linear = direction * ball.ValueRO.velocity;
                localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, math.up());
            }
            else
            {
                physicsVelocity.ValueRW.Linear = new float3(0, 0, 1) * ball.ValueRO.velocity;
            }
            physicsVelocity.ValueRW.Angular = float3.zero;

            if (math.abs(localTransform.ValueRO.Position.x) > 0.001f)
            {
                localTransform.ValueRW.Position.x = 0f; // Constrain to YZ plane
            }
        }
    }
}

