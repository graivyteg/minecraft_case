using UnityEngine;
using Zenject;

namespace UI
{
    public class MoneyText : CustomText
    {
        [SerializeField] private string _format = "{0}";
        [Inject] private Player _player;

        protected override void Start()
        {
            base.Start();
            _player.Wallet.OnBalanceChanged += UpdateDisplay;
            UpdateDisplay(_player.Wallet.Money);
        }

        private void UpdateDisplay(int newValue)
        {
            Text.text = string.Format(_format, newValue);
        }
    }
}