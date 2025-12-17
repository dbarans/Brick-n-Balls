using UnityEngine;
using Unity.Entities;
using Brick;

/// <summary>
/// Authoring component for brick entities.
/// Configures brick health, score value, and type from Unity Editor.
/// </summary>
public class BrickAuthoring : MonoBehaviour
{
    public int Health;
    public int ScoreValue;
    public BrickType Type = BrickType.Hp1;

    public class Baker : Baker<BrickAuthoring>
    {
        public override void Bake(BrickAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            
            // Determine brick type based on health, but respect manually set Type if it matches
            BrickType brickType;
            if (authoring.Health == 1 && authoring.Type != BrickType.Hp1)
                brickType = BrickType.Hp1;
            else if (authoring.Health == 2 && authoring.Type != BrickType.Hp2)
                brickType = BrickType.Hp2;
            else if (authoring.Health >= 3 && authoring.Type != BrickType.Hp3)
                brickType = BrickType.Hp3;
            else
                brickType = authoring.Type; 
            
            AddComponent(entity, new BrickComponent
            {
                Health = authoring.Health,
                ScoreValue = authoring.ScoreValue,
                Type = brickType
            });
        }
    }
}

