
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 0;
        public int chestPrice = 0;
        public int chestsOpened = 0;
        public int bossesKilled = 0;
        public bool isSoundOn = true;
        public bool isMusicOn = true;// Можно задать полям значения по умолчанию
        public List<EntityLevelData> entities;

        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        // Ваши сохранения

        // ...

        // Поля (сохранения) можно удалять и создавать новые. При обновлении игры сохранения ломаться не должны


        // Вы можете выполнить какие то действия при загрузке сохранений
        public SavesYG()
        {
            // Допустим, задать значения по умолчанию для отдельных элементов массива

            entities = new List<EntityLevelData>();
            entities.Add(new EntityLevelData("green_snake"));
            entities[0].Cards = 1;
            openLevels[1] = true;
        }
    }
    
    [System.Serializable]
    public class EntityLevelData
    {
        public string Key;
        public int Level = 1;
        public int Cards = 0;

        public EntityLevelData(string key)
        {
            Key = key;
        }

        public bool CanUpgrade()
        {
            return Cards >= LevelCardsAmount();
        }

        public int LevelCardsAmount()
        {
            return Level + 1;
        }
    }
}
