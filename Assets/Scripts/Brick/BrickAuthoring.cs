using UnityEngine;
using Unity.Entities;

public class BrickAuthoring : MonoBehaviour
{
    public int health;
    public int scoreValue;

    public class Baker : Baker<BrickAuthoring>
    {
        public override void Bake(BrickAuthoring authoring)
        {
           var entity = GetEntity(TransformUsageFlags.Renderable);
            AddComponent(entity, new BrickComponent
            {
                Health = authoring.health,
                ScoreValue = authoring.scoreValue
            });
        }
    }
}

