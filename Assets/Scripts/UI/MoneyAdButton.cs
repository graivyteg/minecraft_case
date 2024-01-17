using System;
using TMPro;
using UI.Buttons;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace UI
{
    public class MoneyAdButton : CustomButton
    {
        [Inject] private EntityManager _entityManager;
        [Inject] private Player _player;

        [SerializeField] private TextMeshProUGUI _moneyText;
        [SerializeField] private Image _timerImage;
        [SerializeField] private float _delay = 60;
        [SerializeField] private int _moneyMultiplier = 50;
        
        private const int MoneyVideoID = 3;
        private float _timer = 0;
        private int _reward = 0;
        
        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnReward;
            UpdateReward();
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnReward;
            _entityManager.SetDamageMultiplier(1);
        }

        protected override void Update()
        {
            base.Update();

            if (_reward == 0) UpdateReward();
            
            _timer -= Mathf.Min(_timer, Time.deltaTime);
            _timerImage.fillAmount = _timer / _delay;
        }

        protected override void OnClick()
        {
            YandexGame.RewVideoShow(MoneyVideoID);
        }
        
        protected override bool IsInteractable()
        {
            return _timer == 0;
        }

        private void OnReward(int videoId)
        {
            if (videoId != MoneyVideoID) return;
            
            _player.Wallet.AddMoney(_reward);
            _timer = _delay;
            UpdateReward();
        }

        private void UpdateReward()
        {
            _reward = _entityManager.GetSummaryDamage() * _moneyMultiplier;
            _moneyText.text = $"+{_reward}";
        }
    }
}