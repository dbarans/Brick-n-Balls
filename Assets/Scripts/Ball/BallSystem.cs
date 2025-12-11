using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct BallSystem : ISystem
{
    
    [BurstCompile] 
    public void OnUpdate(ref SystemState state)
    {
        foreach ((RefRW<LocalTransform> localTransform, RefRW<BallComponent> ball, RefRW<PhysicsVelocity> physicsVelocity)
            in SystemAPI.Query<RefRW<LocalTransform>, RefRW<BallComponent>, RefRW<PhysicsVelocity>>())
        {
            // Wait for fire command
            if (!ball.ValueRO.isFired)
            {
                physicsVelocity.ValueRW.Linear = float3.zero;
                physicsVelocity.ValueRW.Angular = float3.zero;
                return;
            }

            // Fire initialization
            if (!ball.ValueRO.isInitialized)
            {
                float3 rawDir = ball.ValueRO.initialDirection;

                // If vector is empty (0,0,0), use fallback
                if (math.lengthsq(rawDir) < 0.001f)
                {
                    rawDir = math.normalize(new float3(0f, -0.5f, 1f));
                }

                // Lock X axis (gameplay on Y-Z plane)
                rawDir.x = 0;
                rawDir = math.normalize(rawDir);

                // Save and fire
                ball.ValueRW.initialDirection = rawDir;
                ball.ValueRW.isInitialized = true;

                physicsVelocity.ValueRW.Linear = rawDir * ball.ValueRO.velocity;
                physicsVelocity.ValueRW.Angular = float3.zero;

                localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(rawDir, math.up());
            }

            // Ball flight
            else
            {
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
                    // Fallback for stopped ball
                    physicsVelocity.ValueRW.Linear = new float3(0, 0, 1) * ball.ValueRO.velocity;
                }
                physicsVelocity.ValueRW.Angular = float3.zero;

                if (math.abs(localTransform.ValueRO.Position.x) > 0.001f)
                {
                    localTransform.ValueRW.Position.x = 0f;
                }
            }
        }
    }
}
