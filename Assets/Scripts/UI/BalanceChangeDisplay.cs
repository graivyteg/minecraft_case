using System;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class BalanceChangeDisplay : MonoBehaviour
    {
        [Inject] private Player _player;

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private string _triggerName = "Balance Changed";

        private Animator _animator;
        private int _cachedBalance;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _player.Wallet.OnBalanceChanged += OnBalanceChanged;
            _cachedBalance = _player.Wallet.Money;
        }

        private void OnBalanceChanged(int newValue)
        {
            var delta = Mathf.Abs(newValue - _cachedBalance);
            var sign = newValue > _cachedBalance ? "+" : "-";
            var color = newValue > _cachedBalance ? "green" : "red";

            _cachedBalance = newValue;
            
            _text.text = $"<color={color}>{sign}{delta}</color>";
            _animator.SetTrigger(_triggerName);
        }
    }
}