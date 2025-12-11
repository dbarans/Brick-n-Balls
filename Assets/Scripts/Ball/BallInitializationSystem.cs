using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

/// <summary>
/// Handles ball initialization when fired - calculates direction and applies initial velocity.
/// </summary>
[BurstCompile]
partial struct BallInitializationSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<BallComponent> ball, RefRW<PhysicsVelocity> physicsVelocity, RefRW<LocalTransform> localTransform)
            in SystemAPI.Query<RefRW<BallComponent>, RefRW<PhysicsVelocity>, RefRW<LocalTransform>>())
        {
            if (!ball.ValueRO.isFired || ball.ValueRO.isInitialized) continue;

            float3 rawDir = ball.ValueRO.initialDirection;

            if (math.lengthsq(rawDir) < 0.001f)
            {
                rawDir = math.normalize(new float3(0f, -0.5f, 1f));
            }

            rawDir.x = 0; // Lock X axis movement
            rawDir = math.normalize(rawDir);

            ball.ValueRW.initialDirection = rawDir;
            ball.ValueRW.isInitialized = true;

            physicsVelocity.ValueRW.Linear = rawDir * ball.ValueRO.velocity;
            physicsVelocity.ValueRW.Angular = float3.zero;

            localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(rawDir, math.up());
        }
    }
}

