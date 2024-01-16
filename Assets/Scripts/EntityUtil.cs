using YG;

public static class EntityUtil
{
    public static int LevelCardsAmount(EntityLevelData data)
    {
        return data.Level + 1;
    }

    public static bool CanUpgrade(EntityLevelData data)
    {
        return data.Cards >= LevelCardsAmount(data);
    }
    
    public static int GetSaveIndex(string key)
    {
        return YandexGame.savesData.entities.FindIndex(e => e.Key == key);
    }
}