using Unity.Entities;
using UnityEngine;

/// <summary>
/// Authoring component for game score data singleton.
/// Initializes score tracking system in ECS world.
/// </summary>
public class GameDataAuthoring : MonoBehaviour
{
    public class Baker : Baker<GameDataAuthoring>
    {
        public override void Bake(GameDataAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            // PreviousFrameScore = -1 ensures first score change is detected
            AddComponent(entity, new GameScoreData { CurrentScore = 0, PreviousFrameScore = -1 });
        }
    }
}