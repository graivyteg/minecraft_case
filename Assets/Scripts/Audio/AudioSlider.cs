using System;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YG;

namespace Audio
{
    [RequireComponent(typeof(Slider))]
    public class AudioSlider : MonoYandex, IPointerUpHandler
    {
        [SerializeField] private AudioType _type;
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        protected override void OnSDK()
        {
            if (_type == AudioType.Music)
            {
                _slider.value = YandexGame.savesData.music;
            }
            else
            {
                _slider.value = YandexGame.savesData.sound;   
            }
        }
        
        private void Update()
        {
            if (_type == AudioType.Music)
            {
                YandexGame.savesData.music = _slider.value;
            }
            else
            {
                YandexGame.savesData.sound = _slider.value;   
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            YandexGame.SaveProgress();
        }
    }
}