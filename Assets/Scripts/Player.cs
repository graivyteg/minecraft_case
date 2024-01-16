using Settings;
using UnityEngine;
using YG;
using Zenject;

public class Player : IInitializable
{
    public Wallet Wallet = new Wallet();
    
    private int _power = 1;

    public void Initialize()
    {
        if (YandexGame.SDKEnabled) OnSDK();
        else YandexGame.GetDataEvent += OnSDK;

        Wallet.OnBalanceChanged += OnBalanceChanged;
    }

    private void OnSDK()
    {
        Wallet.AddMoney(YandexGame.savesData.money);
    }

    private void OnBalanceChanged(int newValue)
    {
        YandexGame.savesData.money = newValue;
        YandexGame.SaveProgress();
    }

    public int GetPower() => _power;
}