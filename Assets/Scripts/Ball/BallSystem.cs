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
            if (!ball.ValueRO.isInitialized)
            {
                // X must be 0 (Y-Z plane movement only)
                float3 initialDirection = math.normalize(new float3(0f, -0.5f, 1f));

                ball.ValueRW.initialDirection = initialDirection;
                ball.ValueRW.isInitialized = true;

                physicsVelocity.ValueRW.Linear = initialDirection * ball.ValueRO.velocity;
                physicsVelocity.ValueRW.Angular = float3.zero;

                localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(initialDirection, math.up());
            }

            else
            {
                float3 currentVel = physicsVelocity.ValueRO.Linear;

                currentVel.x = 0;

                // Maintain constant velocity after collisions
                if (math.lengthsq(currentVel) > 0.001f)
                {
                    float3 direction = math.normalize(currentVel);

                    // Override physics: use bounce direction but restore original velocity
                    physicsVelocity.ValueRW.Linear = direction * ball.ValueRO.velocity;

                    localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(direction, math.up());
                }
                else
                {
                    // Fallback: restore forward velocity if stopped
                    physicsVelocity.ValueRW.Linear = new float3(0, 0, 1) * ball.ValueRO.velocity;
                }

                // Prevent spinning after collision
                physicsVelocity.ValueRW.Angular = float3.zero;

                // Reset X axis drift
                if (math.abs(localTransform.ValueRO.Position.x) > 0.001f)
                {
                    localTransform.ValueRW.Position.x = 0f;
                }
            }
        }
    }
}
