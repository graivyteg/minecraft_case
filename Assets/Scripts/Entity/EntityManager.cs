using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UI;
using UnityEngine;
using YG;
using Zenject;
using Random = UnityEngine.Random;

public class EntityManager : MonoYandex
{
    [Inject] private Player _player;
    [Inject] private ChestOpener _chestOpener;

    [BoxGroup("Scene Dependent Settings")] 
    [SerializeField] private bool _addInfoButton;
    [BoxGroup("Scene Dependent Settings")] 
    [EnableIf(nameof(_addInfoButton))] 
    [SerializeField] private EntityInfoButton _infoButtonPrefab;
    [BoxGroup("Scene Dependent Settings")] 
    [EnableIf(nameof(_addInfoButton))] 
    [SerializeField] private EntityInfoPopup _infoPopup;
    
    [SerializeField] private List<Transform> _spawnpoints;
    
    private int _lastSpawnpoint = 0;
    private List<EntityData> _entityDatas;
    private List<Entity> _entities = new();

    public Action<string, int> OnCardsAdded;
    public Action<string> OnLevelUpgraded;
    
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
        if (_addInfoButton)
        {
            var button = Instantiate(_infoButtonPrefab, obj.transform);
            button.Initialize(_infoPopup);
        }
        obj.transform.position = GetSpawnPosition();
        var entity = obj.GetComponent<Entity>();
        _entities.Add(entity);
        entity.InitializeData(entityData);

        return entity;
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

    public void Upgrade(string key)
    {
        var entity = GetEntity(key);
        if (!entity.LevelData.CanUpgrade()) return;
        _player.Wallet.TryRemoveMoney(entity.GetUpgradePrice());

        var index = EntityUtil.GetSaveIndex(key);
        YandexGame.savesData.entities[index].Cards -= entity.LevelData.LevelCardsAmount();
        YandexGame.savesData.entities[index].Level++;
        YandexGame.SaveProgress();

        OnLevelUpgraded?.Invoke(key);
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