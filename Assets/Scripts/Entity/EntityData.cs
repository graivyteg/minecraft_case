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
    public Sprite Icon;

    [BoxGroup("Prefabs")] 
    public GameObject Prefab;
}
