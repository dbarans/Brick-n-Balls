using Unity.Entities;
using UnityEngine;

public struct BrickComponent : IComponentData
{
    public int Health;
    public int ScoreValue;
    public BrickType Type;
}

public enum BrickType
{
    Hp1 = 1, 
    Hp2 = 2, 
    Hp3 = 3 
}

public struct BrickPrefabsConfig : IComponentData
{
    public Entity BrickHp1;
    public Entity BrickHp2;
    public Entity BrickHp3;
}

public class BrickVisualLink : IComponentData
{
    public GameObject VisualGameObject;
}
