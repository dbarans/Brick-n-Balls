using Unity.Entities;
using Unity.Physics.Systems;
using Unity.Burst;
using Unity.Physics;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct BrickSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency.Complete();
        var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var brickLookup = SystemAPI.GetComponentLookup<BrickComponent>(false);
        var ballLookup = SystemAPI.GetComponentLookup<BallComponent>(true);
        var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach (var collision in simulation.AsSimulation().CollisionEvents)
        {
            Entity entityA = collision.EntityA;
            Entity entityB = collision.EntityB;

            Entity brickEntity = Entity.Null;

            if (brickLookup.HasComponent(entityA) && ballLookup.HasComponent(entityB))
            {
                brickEntity = entityA;
            }
            else if (brickLookup.HasComponent(entityB) && ballLookup.HasComponent(entityA))
            {
                brickEntity = entityB;
            }
            if (brickEntity != Entity.Null)
            {
                var brickData = brickLookup[brickEntity];
                brickData.Health -= 1;
                if (brickData.Health <= 0)
                {
                    // Destroy visual GO before destroying entity
                    if (state.EntityManager.HasComponent<BrickVisualLink>(brickEntity))
                    {
                        var visualLink = state.EntityManager.GetComponentData<BrickVisualLink>(brickEntity);
                        if (visualLink.VisualGameObject != null)
                        {
                            UnityEngine.Object.Destroy(visualLink.VisualGameObject);
                        }
                    }
                    ecb.DestroyEntity(brickEntity);
                }
                else
                {
                    brickLookup[brickEntity] = brickData;
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
