using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class BallAuthoring : MonoBehaviour
{
    public float velocity;

    public class Baker : Baker<BallAuthoring>
    {
        public override void Bake(BallAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BallComponent
            {
                velocity = authoring.velocity,
                initialDirection = float3.zero,
                isInitialized = false
            });
        }
    }
}
