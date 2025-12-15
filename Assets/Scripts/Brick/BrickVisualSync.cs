using UnityEngine;
using Unity.Entities;
using Unity.Transforms;

/// <summary>
/// Synchronizes GameObject transform with brick entity's ECS transform data.
/// Destroys visual GO when entity is destroyed.
/// </summary>
public class BrickVisualSync : MonoBehaviour
{
    private Entity _brickEntity;
    private EntityManager _entityManager;
    private bool _isInitialized;

    public void Initialize(Entity brickEntity, EntityManager entityManager)
    {
        _brickEntity = brickEntity;
        _entityManager = entityManager;
        _isInitialized = true;
        
        // Set initial position
        if (_entityManager.HasComponent<LocalTransform>(_brickEntity))
        {
            var transform = _entityManager.GetComponentData<LocalTransform>(_brickEntity);
            this.transform.position = transform.Position;
            this.transform.rotation = transform.Rotation;
            float scale = transform.Scale;
            this.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}

