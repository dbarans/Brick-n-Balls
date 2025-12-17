using Unity.Entities;
using UnityEngine;

/// <summary>
/// ECS component data for brick entities.
/// Stores health, score value, and visual type.
/// </summary>
public struct BrickComponent : IComponentData
{
    public int Health;
    public int ScoreValue;
    public BrickType Type;
}

/// <summary>
/// Enum representing brick types based on health points.
/// </summary>
public enum BrickType
{
    Hp1 = 1, 
    Hp2 = 2, 
    Hp3 = 3 
}

/// <summary>
/// Links brick entity to its visual GameObject for synchronization.
/// </summary>
public class BrickVisualLink : IComponentData
{
    public GameObject VisualGameObject;
}
