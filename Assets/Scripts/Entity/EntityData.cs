using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity Data", menuName = "Game/Entity Data", order = 52)]
public class EntityData : ScriptableObject
{
    [BoxGroup("General")] 
    public string Key;
    [BoxGroup("General")]
    public string Name;
    [BoxGroup("General")]
    public Rarity Rarity;
    [BoxGroup("General")]
    [ShowAssetPreview(256, 256)]
    public Sprite Icon;

    [BoxGroup("Characteristics")] 
    public int DefaultDamage = 1;
    [BoxGroup("Characteristics")] 
    public int DamagePerLevel = 1;
    [BoxGroup("Characteristics")] 
    public int FirstUpgradePrice = 10;

    [BoxGroup("Prefabs")] 
    public GameObject Prefab;
}
