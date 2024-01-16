using System;
using System.Collections.Generic;
using UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Case Settings", menuName = "Settings/Case Settings", order = 51)]
public class CaseSettings : ScriptableObject
{
    [Serializable]
    public class FrequencySetting
    {
        public Rarity Rarity;
        public int Frequency = 1;
    }

    public List<FrequencySetting> FrequencySettings;
    public List<CardDisplayPrefab> CardPrefabs;

    public EntityCardDisplay GetCardPrefab(Rarity rarity)
    {
        return CardPrefabs.Find(p => p.Rarity == rarity).Prefab;
    }
}

[Serializable]
public class CardDisplayPrefab
{
    public Rarity Rarity;
    public EntityCardDisplay Prefab;
}

public enum Rarity
{
    Common,
    Rare,
    Epic,
    Legendary
}