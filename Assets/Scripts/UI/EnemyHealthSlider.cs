using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    public class EnemyHealthSlider : CustomSlider
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [Inject] private EnemySpawner _spawner;

        private void Update()
        {
            _healthText.text = $"{Mathf.CeilToInt(_spawner.CurrentMonster.Health)} / {Mathf.CeilToInt(_spawner.CurrentMonster.MaxHealth)}";
            Slider.value = Mathf.Lerp(
                Slider.value, 
                _spawner.CurrentMonster.Health / _spawner.CurrentMonster.MaxHealth, 
                0.2f
            );
        }
    }
}