using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Data
{
    /// <summary>
    /// Provides access to ball entity data in the ECS world.
    /// </summary>
    public interface IEntityDataAccessor
    {
        bool HasBallEntity();
        float3 GetBallPosition();
        quaternion GetBallRotation();
        BallComponent GetBallComponent();
        void SetBallComponent(BallComponent ballData);
        void SetBallInitialDirection(float3 direction);
        void SetBallFired(bool fired);
    }
}

