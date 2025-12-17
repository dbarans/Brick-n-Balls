using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// Destroys balls that fall below the screen and decrements remaining balls count.
/// </summary>
[BurstCompile]
partial struct BallDestructionSystem : ISystem
{
    private const float BallDestructionThreshold = -10f;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        bool ballDestroyed = false;

        foreach ((RefRO<LocalTransform> localTransform, Entity entity)
            in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BallComponent>().WithEntityAccess())
        {
            if (localTransform.ValueRO.Position.y < BallDestructionThreshold)
            {
                ecb.DestroyEntity(entity);
                ballDestroyed = true;
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        if (ballDestroyed && SystemAPI.TryGetSingleton<GameState>(out var gameState))
        {
            gameState.RemainingBalls = math.max(0, gameState.RemainingBalls - 1);
            SystemAPI.SetSingleton(gameState);
        }
    }
}

