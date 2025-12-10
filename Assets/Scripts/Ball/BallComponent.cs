using Unity.Entities;
using Unity.Mathematics;

public struct BallComponent : IComponentData
{
    public float velocity;
    public float3 initialDirection;
    public bool isInitialized;
    public bool isFired;
}