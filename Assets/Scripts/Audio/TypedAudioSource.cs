using System;
using System.Threading;
using UnityEngine;
using YG;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class TypedAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioType _type;
        [SerializeField] private float _multiplier = 1f;

        private bool _isAdPlaying = false;
        private AudioSource _source;
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            
            YandexGame.RewardVideoEvent += OnAdRewarded;
            YandexGame.OpenVideoEvent += OnAdOpened;
            YandexGame.CloseVideoEvent += OnAdClosed;
            YandexGame.ErrorVideoEvent += OnAdFailed;
        }

        protected void Update()
        {
            if (_source == null) return;
            _source.volume = GetVolume();
        }

        private void OnAdRewarded(int id)
        {
            _isAdPlaying = false;
        }

        private void OnAdFailed()
        {
            _isAdPlaying = false;
        }

        private void OnAdClosed()
        {
            _isAdPlaying = false;
        }

        private void OnAdOpened()
        {
            _isAdPlaying = true;
        }

        private float GetVolume()
        {
            if (_isAdPlaying) return 0;
            if (!YandexGame.SDKEnabled) return 0;
            if (_type == AudioType.Sound && !YandexGame.savesData.isSoundOn) return 0;
            if (_type == AudioType.Music && !YandexGame.savesData.isMusicOn) return 0;
            
            return _multiplier;
        }
    }
}