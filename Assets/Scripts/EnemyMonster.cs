using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

[RequireComponent(typeof(Animator))]
public class EnemyMonster : MonoBehaviour
{
    [Inject] private FightSettings _fightSettings;

    public float MaxHealth { get; private set; }
    public float Health { get; private set; }
    
    private Animator _animator;

    public Action OnDied;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        MaxHealth = _fightSettings.HealthByDamageMultiplier * 1;
        Health = MaxHealth;

        var defaultScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(defaultScale, 0.5f);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DealDamage();
        }
    }

    private void DealDamage()
    {
        if (Health <= 0) return;
        
        _animator.SetTrigger("Take Damage");

        Health -= Mathf.Min(Health, 1);
        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _animator.SetTrigger("Die");
        transform.DOScale(Vector3.zero, 0.5f).OnComplete(() =>
        {
            OnDied?.Invoke();
            Destroy(gameObject);
        });
    }
}