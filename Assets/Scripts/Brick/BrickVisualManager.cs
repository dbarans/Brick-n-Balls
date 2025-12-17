using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using System.Collections;

/// <summary>
/// Creates and manages visual GameObjects for brick entities.
/// Waits for ECS world to be ready before initializing visuals to ensure all bricks are baked.
/// </summary>
public class BrickVisualManager : MonoBehaviour
{
    [Header("Sprite Prefabs")]
    public GameObject BrickHp1SpritePrefab;
    public GameObject BrickHp2SpritePrefab;
    public GameObject BrickHp3SpritePrefab;

    private bool _initialized = false;

    private void Start()
    {
        StartCoroutine(InitializeWhenReady());
    }

    /// <summary>
    /// Waits until ECS world is ready and all brick entities are baked before creating visuals.
    /// </summary>
    private IEnumerator InitializeWhenReady()
    {
        yield return new WaitUntil(IsReadyToInitialize);
        CreateVisualsForAllBricks();
    }

    /// <summary>
    /// Checks if ECS world is ready, prefabs are assigned, and brick entities exist.
    /// </summary>
    private bool IsReadyToInitialize()
    {
        if (_initialized) return true;

        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null || !world.IsCreated) return false;

        if (BrickHp1SpritePrefab == null || BrickHp2SpritePrefab == null || BrickHp3SpritePrefab == null)
            return false;

        var entityManager = world.EntityManager;
        var query = entityManager.CreateEntityQuery(typeof(BrickComponent), typeof(LocalTransform));
        return !query.IsEmpty;
    }

    /// <summary>
    /// Creates visual GameObjects for all brick entities that don't have visuals yet.
    /// Links visuals to entities via BrickVisualLink component for synchronization.
    /// </summary>
    private void CreateVisualsForAllBricks()
    {
        if (_initialized) return;

        var world = World.DefaultGameObjectInjectionWorld;
        var entityManager = world.EntityManager;
        var query = entityManager.CreateEntityQuery(typeof(BrickComponent), typeof(LocalTransform));

        using (var entities = query.ToEntityArray(Unity.Collections.Allocator.Temp))
        {
            foreach (var entity in entities)
            {
                // Skip bricks that already have visuals
                if (entityManager.HasComponent<BrickVisualLink>(entity))
                    continue;

                var brick = entityManager.GetComponentData<BrickComponent>(entity);
                var transform = entityManager.GetComponentData<LocalTransform>(entity);

                GameObject prefab = null;
                switch (brick.Type)
                {
                    case BrickType.Hp1:
                        prefab = BrickHp1SpritePrefab;
                        break;
                    case BrickType.Hp2:
                        prefab = BrickHp2SpritePrefab;
                        break;
                    case BrickType.Hp3:
                        prefab = BrickHp3SpritePrefab;
                        break;
                }

                if (prefab == null)
                {
                    Debug.LogWarning($"BrickVisualManager: Missing prefab for type {brick.Type}");
                    continue;
                }

                GameObject visual = Instantiate(prefab);
                visual.name = $"BrickVisual_{entity.Index}_{brick.Type}";
                visual.transform.position = transform.Position;
                visual.transform.rotation = transform.Rotation;
                float scale = transform.Scale;
                visual.transform.localScale = new Vector3(scale, scale, scale);

                var sync = visual.GetComponent<BrickVisualSync>();
                if (sync == null)
                {
                    sync = visual.AddComponent<BrickVisualSync>();
                }
                sync.Initialize(entity, entityManager);

                entityManager.AddComponentData(entity, new BrickVisualLink
                {
                    VisualGameObject = visual
                });
            }
        }
        
        _initialized = true;
    }
}

