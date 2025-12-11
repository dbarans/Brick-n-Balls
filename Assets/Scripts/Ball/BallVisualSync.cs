using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Data;

/// <summary>
/// Synchronizes GameObject transform with ball entity's ECS transform data.
/// </summary>
public class BallVisualSync : MonoBehaviour
{
    private IEntityDataAccessor _entityDataAccessor;

    public void Initialize(IEntityDataAccessor entityDataAccessor)
    {
        _entityDataAccessor = entityDataAccessor;
    }

    void Start()
    {
        if (_entityDataAccessor == null)
        {
            var world = World.DefaultGameObjectInjectionWorld;
            _entityDataAccessor = new EntityDataAccessor(world.EntityManager);
        }
    }

    void LateUpdate()
    {
        if (_entityDataAccessor?.HasBallEntity() == true)
        {
            transform.position = _entityDataAccessor.GetBallPosition();
            transform.rotation = _entityDataAccessor.GetBallRotation();
        }
    }
}
