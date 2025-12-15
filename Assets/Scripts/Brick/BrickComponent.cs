using Unity.Entities;
using UnityEngine;

public struct BrickComponent : IComponentData
{
    public int Health;
    public int ScoreValue;
}
public struct BrickPrefabsConfig : IComponentData
{
    public Entity BrickHp1;
    public Entity BrickHp2;
    public Entity BrickHp3;
}
public enum BrickType
{
    Hp1, 
    Hp2, 
    Hp3 
}

public class VisualLinkComponent : IComponentData
{
    public GameObject VisualGameObject;
}