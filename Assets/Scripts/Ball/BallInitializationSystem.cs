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
    private const float DirectionEpsilon = 0.001f;
    private static readonly float3 DefaultDirection = new float3(0f, -0.5f, 1f);

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<BallComponent> ball, RefRW<PhysicsVelocity> physicsVelocity, RefRW<LocalTransform> localTransform)
            in SystemAPI.Query<RefRW<BallComponent>, RefRW<PhysicsVelocity>, RefRW<LocalTransform>>())
        {
            if (!ball.ValueRO.IsFired || ball.ValueRO.IsInitialized) continue;

            float3 rawDir = ball.ValueRO.InitialDirection;

            if (math.lengthsq(rawDir) < DirectionEpsilon)
            {
                rawDir = math.normalize(DefaultDirection);
            }

            rawDir.x = 0; // Lock X axis movement
            rawDir = math.normalize(rawDir);

            ball.ValueRW.InitialDirection = rawDir;
            ball.ValueRW.IsInitialized = true;

            physicsVelocity.ValueRW.Linear = rawDir * ball.ValueRO.Velocity;
            physicsVelocity.ValueRW.Angular = float3.zero;

            localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(rawDir, math.up());
        }
    }
}

