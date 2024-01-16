using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fight Settings", menuName = "Settings/Fight Settings", order = 51)]
public class FightSettings : ScriptableObject
{
    [Tooltip("На сколько умножается урон при вычислении здоровья босса")]
    public int HealthByDamageMultiplier = 250;
}