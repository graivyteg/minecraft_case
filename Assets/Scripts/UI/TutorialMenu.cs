using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace UI
{
    [RequireComponent(typeof(CustomMenu))]
    public class TutorialMenu : MonoYandex
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _tutorialText;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [Header("Data")] 
        [TextArea]
        [SerializeField] private List<string> _texts;

        private CustomMenu _menu;
        private int _index = 0;

        private void Awake()
        {
            _menu = GetComponent<CustomMenu>();
            _nextButton.onClick.AddListener(Next);
            _previousButton.onClick.AddListener(Previous);
        }

        protected override void OnSDK()
        {
            if (!YandexGame.savesData.tutorialCompleted)
            {
                _menu.SetActive(true);
                UpdateDisplay();
            }
        }

        private void Next()
        {
            _index++;

            if (_index >= _texts.Count)
            {
                _menu.SetActive(false);
                YandexGame.savesData.tutorialCompleted = true;
                YandexGame.SaveProgress();
                return;
            }
            
            UpdateDisplay();
        }

        private void Previous()
        {
            if (_index == 0) return;
            _index--;
            
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            _previousButton.interactable = _index > 0;
            _tutorialText.text = _texts[_index];
        }

        public void ShowTutorial()
        {
            _menu.SetActive(true);
            _index = 0;
            UpdateDisplay();
        }
    }
}