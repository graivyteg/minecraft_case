using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using YG;
using Zenject;

[RequireComponent(typeof(Animator))]
public class EnemyMonster : MonoYandex
{
    [Inject] private Player _player;
    [Inject] private FightSettings _fightSettings;
    [Inject] private EntityManager _entityManager;

    public float MaxHealth { get; private set; }
    public float Health { get; private set; }
    
    private Animator _animator;

    public Action OnDied;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    protected override void OnSDK()
    {
        var multiplier = 1 + YandexGame.savesData.bossesKilled * _fightSettings.HealthByBossNumberMultiplier;
        MaxHealth = Mathf.RoundToInt(_fightSettings.HealthByDamageMultiplier * _entityManager.GetSummaryDamage() * multiplier);
        Health = MaxHealth;
    }

    private void Start()
    {
        var defaultScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(defaultScale, 0.5f);
    }
    
    public void DealDamage()
    {
        if (Health <= 0) return;
        
        var damage = _entityManager.GetSummaryDamage();
        _animator.SetTrigger("Take Damage");
        
        _player.Wallet.AddMoney(damage);

            Health -= Mathf.Min(Health, damage);
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        YandexGame.savesData.bossesKilled++;
        YandexGame.NewLeaderboardScores("bosskills", YandexGame.savesData.bossesKilled);
        YandexGame.SaveProgress();
        
        _animator.SetTrigger("Die");
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            OnDied?.Invoke();
            Destroy(gameObject);
        });
    }
}