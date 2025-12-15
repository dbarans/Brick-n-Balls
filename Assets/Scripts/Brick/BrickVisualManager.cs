using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class BrickVisualManager : MonoBehaviour
{
    [Header("Sprite Prefabs")]
    public GameObject BrickHp1SpritePrefab;
    public GameObject BrickHp2SpritePrefab;
    public GameObject BrickHp3SpritePrefab;

    private void Start()
    {
        CreateVisualsForAllBricks();
    }

    private void CreateVisualsForAllBricks()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null) return;

        var entityManager = world.EntityManager;
        var query = entityManager.CreateEntityQuery(typeof(BrickComponent), typeof(LocalTransform));

        foreach (var entity in query.ToEntityArray(Unity.Collections.Allocator.Temp))
        {
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
}

