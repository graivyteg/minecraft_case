using YG;

public static class EntityUtil
{
    public static int GetSaveIndex(string key)
    {
        return YandexGame.savesData.entities.FindIndex(e => e.Key == key);
    }
}