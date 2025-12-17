using UnityEngine;
using Unity.Entities;

/// <summary>
/// Authoring component for ball entities.
/// Creates ball entities with default uninitialized state.
/// </summary>
public class BallAuthoring : MonoBehaviour
{
    public class Baker : Baker<BallAuthoring>
    {
        public override void Bake(BallAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BallComponent
            {
                Velocity = 0f,
                IsInitialized = false
            });
        }
    }
}
