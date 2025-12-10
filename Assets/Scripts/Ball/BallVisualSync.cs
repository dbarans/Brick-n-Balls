using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class BallVisualSync : MonoBehaviour
{
    private EntityManager _entityManager;
    private EntityQuery _ballQuery;

    void Start()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        _entityManager = world.EntityManager;

        _ballQuery = _entityManager.CreateEntityQuery(
            typeof(LocalTransform),
            typeof(BallComponent)
        );
    }

    void LateUpdate()
    {
        // LateUpdate prevents jittering (syncs after physics)
        if (!_ballQuery.IsEmpty)
        {
            var entity = _ballQuery.GetSingletonEntity();

            var localTransform = _entityManager.GetComponentData<LocalTransform>(entity);

            transform.position = localTransform.Position;
            transform.rotation = localTransform.Rotation;
        }
        else
        {

        }
    }
}
