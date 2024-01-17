using System;
using UI.Buttons;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

namespace UI
{
    public class DoubleDamageButton : CustomButton
    {
        [Inject] private EntityManager _entityManager;
        
        [SerializeField] private Image _timerImage;
        [SerializeField] private float _doubleDamageTime = 60;
        
        private const int DoubleDamageVideoID = 2;
        private float _timer = 0;

        private CompositeDisposable _disposable = new();
        
        private void OnEnable()
        {
            YandexGame.RewardVideoEvent += OnReward;
        }

        private void OnDisable()
        {
            YandexGame.RewardVideoEvent -= OnReward;
            _entityManager.SetDamageMultiplier(1);
        }

        protected override void Update()
        {
            base.Update();

            _timer -= Mathf.Min(_timer, Time.deltaTime);
            _timerImage.fillAmount = _timer / _doubleDamageTime;
        }

        protected override void OnClick()
        {
            YandexGame.RewVideoShow(DoubleDamageVideoID);
        }
        
        protected override bool IsInteractable()
        {
            return _timer == 0;
        }

        private void OnReward(int videoId)
        {
            if (videoId != DoubleDamageVideoID) return;

            _timer = _doubleDamageTime;
            
            _entityManager.SetDamageMultiplier(2);
            Observable.Timer(TimeSpan.FromSeconds(_doubleDamageTime)).Subscribe(_ =>
            {
                _entityManager.SetDamageMultiplier(1);
            }).AddTo(_disposable);
        }
    }
}