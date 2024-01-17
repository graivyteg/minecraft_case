using System;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class EntityInfoPopup : CustomMenu
    {
        [Inject] private Player _player;
        [Inject] private CaseSettings _caseSettings;
        [Inject] private EntityManager _entityManager;
        [Inject] private SoundController _soundController;
        
        [SerializeField] private Transform _cardContainer;
        [SerializeField] private TextMeshProUGUI _damageText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private TextMeshProUGUI _cardsText;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TextMeshProUGUI _upgradeButtonText;
        [SerializeField] private AudioClip _showSound;
        [SerializeField] private AudioClip _upgradeSound;

        private Entity _entity;

        protected override void Start()
        {
            base.Start();
            _upgradeButton.onClick.AddListener(Upgrade);
        }

        private void OnEnable()
        {
            _entityManager.OnLevelUpgraded += OnLevelUpgraded;
        }

        private void OnDisable()
        {
            _entityManager.OnLevelUpgraded -= OnLevelUpgraded;
        }

        public void Show(Entity entity)
        {
            SetActive(true);
            _soundController.Play(_showSound);
            _entity = entity;
            UpdateDisplay();
        }

        private void OnLevelUpgraded(string key)
        {
            if (_entity != null && _entity.Data.Key == key) UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var levelData = _entity.LevelData;
            
            if (_cardContainer.childCount > 0)
            {
                Destroy(_cardContainer.GetChild(0).gameObject);
            }
            
            var obj = Instantiate(_caseSettings.GetCardPrefab(_entity.Data.Rarity), _cardContainer);
            obj.Initialize(_entity.Data);

            _levelText.text = levelData.Level.ToString();
            _cardsText.text = $"Карты: {levelData.Cards}/{levelData.LevelCardsAmount()}";
            _damageText.text = $"Урон: {_entity.GetDamage()}";
            _upgradeButtonText.text = $"Улучшить\n({_entity.GetUpgradePrice()})";
            
            if (levelData.CanUpgrade())
            {
                _damageText.text += $" <color=green>(+{_entity.Data.DamagePerLevel})</color>";
            }

            _upgradeButton.interactable =
                _player.Wallet.Money >= _entity.GetUpgradePrice() &&
                levelData.CanUpgrade();
        }

        private void Upgrade()
        {
            _soundController.Play(_upgradeSound);
            _entityManager.Upgrade(_entity.Data.Key);
        }
    }
}