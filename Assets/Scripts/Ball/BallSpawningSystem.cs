using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct BallSpawningSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<GameState>(out var gameState)) return;

        var ballQuery = SystemAPI.QueryBuilder().WithAll<BallComponent>().Build();
        if (!ballQuery.IsEmpty) return;

        if (gameState.RemainingBalls <= 0) return;

        var newBall = state.EntityManager.Instantiate(gameState.BallPrefab);
        state.EntityManager.SetComponentData(newBall, LocalTransform.FromPosition(gameState.BallStartPosition));

        state.EntityManager.SetComponentData(newBall, new BallComponent
        {
            velocity = gameState.BallVelocity,
            isFired = false,
            isInitialized = false,
            initialDirection = float3.zero
        });
    }
}
