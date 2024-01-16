using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace UI
{
    public class OpenChestPopup : CustomMenu
    {
        [Inject] private Player _player;
        [Inject] private ChestOpener _chestOpener;

        [Header("References")] 
        [SerializeField] private CustomMenu _chestOpenerMenu;
        [SerializeField] private Button _openButton;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _notEnoughMoneyText;
        [SerializeField] private TextMeshProUGUI _openButtonText;

        [Header("Texts")] 
        [SerializeField] private string _priceFormat = "Цена: {0}";

        private int _chestPrice => YandexGame.savesData.chestPrice;
        
        private const int ChestRewardID = 0;
        
        protected override void Start()
        {
            base.Start();
            _openButton.onClick.AddListener(OnOpenClick);
            YandexGame.RewardVideoEvent += OnRewardedVideo;
        }

        private void Update()
        {
            if (!YandexGame.SDKEnabled) return;
            
            var priceTextValue = "Бесплатно";
            if (_chestPrice > 0) priceTextValue = _chestPrice.ToString();
            _priceText.text = string.Format(_priceFormat, priceTextValue);
            
            var isEnough = _player.Wallet.Money >= _chestPrice;
            
            _openButtonText.text = isEnough ? "Открыть" : "Открыть за рекламу";
            _notEnoughMoneyText.gameObject.SetActive(!isEnough);
        }

        private void OnOpenClick()
        {
            if (_player.Wallet.Money >= _chestPrice)
            {
                _player.Wallet.TryRemoveMoney(YandexGame.savesData.chestPrice);
                OpenChest();
            }
            else
            {
                YandexGame.RewVideoShow(ChestRewardID);
            }
        }
        
        private void OnRewardedVideo(int videoId)
        {
            if (videoId == ChestRewardID) OpenChest();
        }

        private void OpenChest()
        {
            SetActive(false);
            _chestOpenerMenu.SetActive(true);
            _chestOpener.Open();
        }
    }
}