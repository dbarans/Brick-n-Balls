using UnityEngine;
using Unity.Entities;

public class BallAuthoring : MonoBehaviour
{
    public class Baker : Baker<BallAuthoring>
    {
        public override void Bake(BallAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new BallComponent
            {
                velocity = 0f,
                isInitialized = false
            });
        }
    }
}
