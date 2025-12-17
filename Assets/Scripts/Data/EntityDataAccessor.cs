using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Ball;

namespace Data
{
    /// <summary>
    /// Provides access to ball entity data in the ECS world.
    /// Assumes only one ball entity exists at a time.
    /// </summary>
    public class EntityDataAccessor : IEntityDataAccessor
{
    private EntityManager _entityManager;
    private EntityQuery _ballQuery;

    public EntityDataAccessor(EntityManager entityManager)
    {
        _entityManager = entityManager;
        _ballQuery = _entityManager.CreateEntityQuery(
            typeof(BallComponent),
            typeof(LocalTransform)
        );
    }

    public bool HasBallEntity()
    {
        return !_ballQuery.IsEmpty;
    }

    public float3 GetBallPosition()
    {
        if (_ballQuery.IsEmpty) return float3.zero;
        var entity = _ballQuery.GetSingletonEntity();
        var transform = _entityManager.GetComponentData<LocalTransform>(entity);
        return transform.Position;
    }

    public quaternion GetBallRotation()
    {
        if (_ballQuery.IsEmpty) return quaternion.identity;
        var entity = _ballQuery.GetSingletonEntity();
        var transform = _entityManager.GetComponentData<LocalTransform>(entity);
        return transform.Rotation;
    }

    public BallComponent GetBallComponent()
    {
        if (_ballQuery.IsEmpty) return default;
        var entity = _ballQuery.GetSingletonEntity();
        return _entityManager.GetComponentData<BallComponent>(entity);
    }

    public void SetBallComponent(BallComponent ballData)
    {
        if (_ballQuery.IsEmpty) return;
        var entity = _ballQuery.GetSingletonEntity();
        _entityManager.SetComponentData(entity, ballData);
    }

    public void SetBallInitialDirection(float3 direction)
    {
        if (_ballQuery.IsEmpty) return;
        var entity = _ballQuery.GetSingletonEntity();
        var ballData = _entityManager.GetComponentData<BallComponent>(entity);
        ballData.InitialDirection = direction;
        _entityManager.SetComponentData(entity, ballData);
    }

    public void SetBallFired(bool fired)
    {
        if (_ballQuery.IsEmpty) return;
        var entity = _ballQuery.GetSingletonEntity();
        var ballData = _entityManager.GetComponentData<BallComponent>(entity);
        ballData.IsFired = fired;
        _entityManager.SetComponentData(entity, ballData);
    }

    public Entity GetBallEntity()
    {
        if (_ballQuery.IsEmpty) return Entity.Null;
        return _ballQuery.GetSingletonEntity();
    }
}
}
