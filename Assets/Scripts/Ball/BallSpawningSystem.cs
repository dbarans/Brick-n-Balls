using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Game;

namespace Ball
{
    /// <summary>
    /// Spawns a new ball when no balls are present on the field
    /// </summary>
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    partial struct BallSpawningSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.TryGetSingleton<GameState>(out var gameState)) return;

        // Don't spawn balls until game is started
        if (!gameState.IsGameStarted) return;

        var ballQuery = SystemAPI.QueryBuilder().WithAll<BallComponent>().Build();
        if (!ballQuery.IsEmpty) return;

        if (gameState.RemainingBalls <= 0) return;

        var newBall = state.EntityManager.Instantiate(gameState.BallPrefab);
        state.EntityManager.SetComponentData(newBall, LocalTransform.FromPosition(gameState.BallStartPosition));

        state.EntityManager.SetComponentData(newBall, new BallComponent
        {
            Velocity = gameState.BallVelocity,
            IsFired = false,
            IsInitialized = false,
            InitialDirection = float3.zero
        });
    }
}
}
