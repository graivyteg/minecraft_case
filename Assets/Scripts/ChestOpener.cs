using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using Settings;
using UI;
using UnityEngine;
using YG;
using Zenject;
using Random = UnityEngine.Random;

public class ChestOpener : CustomMenu
{
    [Inject] private CaseSettings _caseSettings;
    [Inject] private Player _player;
    [Inject] private EntityManager _manager;
    
    [Header("Settings")]
    public ChestData ChestData;
    [Header("References")]
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _viewport;
    [Header("Other")]
    [SerializeField] private int _cardsAmount = 40;
    [SerializeField] private float _scrollTime = 5;
    [MinMaxSlider(0f, 1f)]
    [SerializeField] private Vector2 _dropPosition;

    private List<EntityCardDisplay> _cards = new();

    public Action<EntityData, int> OnChestOpened;

    [Button]
    public void Open()
    {
        YandexGame.savesData.chestsOpened++;
        UpdateChestPrice();
        
        _content.anchoredPosition = new Vector2(0, _content.anchoredPosition.y);
        StartCoroutine(GenerateCards(() =>
        {
            var cardId = Mathf.RoundToInt(
                Random.Range((_cards.Count - 1) * _dropPosition.x, (_cards.Count - 1) * _dropPosition.y)
            );
            Debug.Log("Card Id " + cardId);
            
            var normalizedCardPos = Random.Range(0f, 1f);

            var cardSize = _content.rect.width / _content.childCount;
            var viewportHalf = _viewport.rect.width / 2;

            var targetPosition = viewportHalf - cardSize * (cardId + normalizedCardPos);
            _content.DOAnchorPosX(targetPosition, _scrollTime).SetEase(Ease.OutCirc).OnComplete(() =>
            {
                var amount = Random.Range(ChestData.MinDropAmount, ChestData.MaxDropAmount + 1);
                OnChestOpened?.Invoke(_cards[cardId].Data, amount);
                SetActive(false);
            });
        }));
    }

    private IEnumerator GenerateCards(Action callback)
    {
        while (_cards.Count > 0)
        {
            Destroy(_cards[0].gameObject);
            _cards.RemoveAt(0);
        }

        _content.anchoredPosition = new Vector2(0, _content.anchoredPosition.y);
        
        for (int i = 0; i < _cardsAmount; i++)
        {
            var obj = SpawnCard();
            _cards.Add(obj);
            yield return null;
        }
        
        callback?.Invoke();
    }

    public EntityCardDisplay SpawnCard()
    {
        var rarities = ChestData.Entities.Select(e => e.Rarity).Distinct().ToList();
        var frSettings = _caseSettings.FrequencySettings.Where(s => rarities.Contains(s.Rarity)).ToList();
        var allFrequency = frSettings.Sum(s => s.Frequency);
        var randomValue = Random.Range(0, allFrequency);

        var resultRarity = Rarity.Common;
        for (int i = 0; i < frSettings.Count; i++)
        {
            if (randomValue <= frSettings[i].Frequency)
            {
                resultRarity = frSettings[i].Rarity;
                break;
            }

            randomValue -= frSettings[i].Frequency;
        }
        
        var prefab = _caseSettings.GetCardPrefab(resultRarity);
        var entityDataList = ChestData.Entities.Where(e => e.Rarity == resultRarity).ToList();
        var entityData = entityDataList[Random.Range(0, entityDataList.Count)];
        var obj = Instantiate(prefab, _content); 
        obj.Initialize(entityData);
        
        return obj;
    }
    
    private void UpdateChestPrice()
    {
        var newPrice = Mathf.RoundToInt(_player.GetPower() * ChestData.ClickAmount * (1 + YandexGame.savesData.chestsOpened * 0.1f));
        YandexGame.savesData.chestPrice = newPrice;
        YandexGame.SaveProgress();
    }
}