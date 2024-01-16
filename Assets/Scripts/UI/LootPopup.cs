using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace UI
{
    public class LootPopup : CustomMenu
    {
        [Inject] private EntityManager _entityManager;
        [Inject] private CaseSettings _caseSettings;

        [SerializeField] private Transform _cardContainer;
        [SerializeField] private Button _doubleButton;
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private TextMeshProUGUI _upgradeText;

        private EntityData _currentData;
        private int _currentAmount = 0;
        private const int DoubleAdId = 1;
        private bool _isDoubled = false;

        protected override void Start()
        {
            base.Start();
            _doubleButton.onClick.AddListener(() =>
            {
                YandexGame.RewVideoShow(DoubleAdId);
            });
        }

        private void OnEnable()
        {
            _entityManager.OnCardsAdded += ShowLoot;
            YandexGame.RewardVideoEvent += OnRewarded;
        }

        private void OnDisable()
        {
            _entityManager.OnCardsAdded -= ShowLoot;
            YandexGame.RewardVideoEvent -= OnRewarded;
        }

        public void ShowLoot(string key, int amount)
        {
            _currentData = _entityManager.GetData(key);
            _currentAmount = amount;
            
            SetActive(true);
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (_cardContainer.childCount > 0)
            {
                Destroy(_cardContainer.GetChild(0).gameObject);
            }

            _doubleButton.interactable = !_isDoubled;

            var obj = Instantiate(_caseSettings.GetCardPrefab(_currentData.Rarity), _cardContainer);
            obj.Initialize(_currentData);
            
            var levelData = _entityManager.GetEntity(_currentData.Key).LevelData;
            var amount = _isDoubled ? _currentAmount * 2 : _currentAmount; 
            _amountText.text = $"+{amount} ({levelData.Cards}/{levelData.LevelCardsAmount()})";
            bool canUpgrade = levelData.CanUpgrade();
            _upgradeText.gameObject.SetActive(canUpgrade);

            _isDoubled = false;
        }

        private void OnRewarded(int id)
        {
            if (id != DoubleAdId) return;

            _isDoubled = true;
            _entityManager.AddCards(_currentData.Key, _currentAmount);
        }
    }
}