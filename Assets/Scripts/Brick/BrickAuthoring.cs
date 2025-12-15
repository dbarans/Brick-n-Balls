using UnityEngine;
using Unity.Entities;

public class BrickAuthoring : MonoBehaviour
{
    public int health;
    public int scoreValue;
    public BrickType type = BrickType.Hp1;

    public class Baker : Baker<BrickAuthoring>
    {
        public override void Bake(BrickAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            BrickType brickType = authoring.type;
            if (authoring.health == 1 && authoring.type != BrickType.Hp1)
                brickType = BrickType.Hp1;
            else if (authoring.health == 2 && authoring.type != BrickType.Hp2)
                brickType = BrickType.Hp2;
            else if (authoring.health >= 3 && authoring.type != BrickType.Hp3)
                brickType = BrickType.Hp3;
            
            AddComponent(entity, new BrickComponent
            {
                Health = authoring.health,
                ScoreValue = authoring.scoreValue,
                Type = brickType
            });
        }
    }
}

