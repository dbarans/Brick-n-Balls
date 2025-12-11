using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class BallVisualSync : MonoBehaviour
{
    private IEntityDataAccessor _entityDataAccessor;

    void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        _entityDataAccessor = new EntityDataAccessor(world.EntityManager);
    }

    void LateUpdate()
    {
        if (_entityDataAccessor.HasBallEntity())
        {
            transform.position = _entityDataAccessor.GetBallPosition();
            transform.rotation = _entityDataAccessor.GetBallRotation();
        }
    }
}
