using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;
using Zenject;
using Random = UnityEngine.Random;

public class EntityManager : MonoYandex
{
    [Inject] private ChestOpener _chestOpener;
    
    [SerializeField] private List<Transform> _spawnpoints;
    
    private int _lastSpawnpoint = 0;
    private List<EntityData> _entityDatas;
    private List<Entity> _entities = new();

    public Action<string, int> OnCardsAdded;
    
    private void Awake()
    {
        _entityDatas = Resources.LoadAll<EntityData>("Cards/").ToList();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _chestOpener.OnChestOpened += OnChestOpened;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _chestOpener.OnChestOpened -= OnChestOpened;
    }

    protected override void OnSDK()
    {
        foreach (var pair in YandexGame.savesData.entities)
        {
            Spawn(GetData(pair.Key));
        }
    }

    public Entity Spawn(EntityData entityData)
    {
        var prefab = entityData.Prefab;
        var obj = Instantiate(prefab);
        obj.transform.position = GetSpawnPosition();
        var entity = obj.GetComponent<Entity>();
        _entities.Add(entity);
        entity.InitializeData(entityData);

        return entity;
    }

    public void AddCards(string key, int amount)
    {
        var index = EntityUtil.GetSaveIndex(key);
        if (index == -1)
        {
            YandexGame.savesData.entities.Add(new EntityLevelData(key));
            index = YandexGame.savesData.entities.Count - 1;
            Spawn(GetData(key));
        }

        var entity = _entities.Find(e => e.Data.Key == key);
        YandexGame.savesData.entities[index].Cards += amount;
        YandexGame.SaveProgress();
        OnCardsAdded?.Invoke(key, amount);
        
    }

    private Vector3 GetSpawnPosition()
    {
        var randomValue = Random.Range(0, _spawnpoints.Count - 1);
        if (randomValue >= _lastSpawnpoint) randomValue++;

        return _spawnpoints[randomValue].position;
    }

    private void OnChestOpened(EntityData data, int amount)
    {
        AddCards(data.Key, amount);
    }
    
    public Entity GetEntity(string key)
    {
        return _entities.Find(e => e.Data.Key == key);  
    }

    public EntityData GetData(string key)
    {
        return _entityDatas.Find(d => d.Key == key);
    }
}