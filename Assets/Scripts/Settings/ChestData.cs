using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Chest Data", menuName = "Game/Chest Data", order = 52)]
    public class ChestData : ScriptableObject
    {
        [Tooltip("Кол-во кликов для открытия сундука")]
        public int ClickAmount = 100;

        public int MinDropAmount = 1;
        public int MaxDropAmount = 3;

        public List<EntityData> Entities;
    }
}