using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class GameStateAuthoring : MonoBehaviour
{
    public GameObject BallPrefab;
    public int StartingBalls = 3;
    public float3 StartPosition = new float3(0, -3.5f, 0);
    public float BallVelocity = 5f;

    class Baker : Baker<GameStateAuthoring>
    {
        public override void Bake(GameStateAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new GameState
            {
                BallPrefab = GetEntity(authoring.BallPrefab, TransformUsageFlags.Dynamic),
                RemainingBalls = authoring.StartingBalls,
                BallStartPosition = authoring.StartPosition,
                BallVelocity = authoring.BallVelocity
            });
        }
    }
}
