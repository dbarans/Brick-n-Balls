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
            // Initialize ball velocity only once
            if (!ball.ValueRO.isInitialized)
            {
                // Set initial direction (e.g., forward and slightly down for shooting)
                float3 initialDirection = math.normalize(new float3(0f, -0.2f, 1f)); // Adjust as needed
                
                ball.ValueRW.initialDirection = initialDirection;
                ball.ValueRW.isInitialized = true;
                
                // Set initial velocity - physics will handle bounces automatically
                physicsVelocity.ValueRW.Linear = initialDirection * ball.ValueRO.velocity;
                physicsVelocity.ValueRW.Angular = float3.zero;
                
                // Set rotation to face movement direction
                localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(initialDirection, math.up());
            }
            else
            {
                // After initialization, let Unity Physics handle collisions and bounces
                // Only update rotation to face current velocity direction
                float3 currentVelocity = physicsVelocity.ValueRO.Linear;
                if (math.lengthsq(currentVelocity) > 0.0001f)
                {
                    float3 velocityDirection = math.normalize(currentVelocity);
                    localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(velocityDirection, math.up());
                }
            }
        }
    }
}
