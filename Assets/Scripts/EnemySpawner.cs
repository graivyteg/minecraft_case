using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Inject] private DiContainer _container;
    [Inject] private SoundController _soundController;
    
    [SerializeField] private List<EnemyMonster> _enemyPrefabs;
    [SerializeField] private Button _damageButton;
    [SerializeField] private AudioClip _damageClip;

    public EnemyMonster CurrentMonster { get; private set; }
    
    private void Start()
    {
        SpawnMonster();
        _damageButton.onClick.AddListener(Damage);
    }

    private void Damage()
    {
        if (CurrentMonster == null) return;
        
        _soundController.Play(_damageClip);
        CurrentMonster.DealDamage();
    }

    private void SpawnMonster()
    {
        var prefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
        CurrentMonster = _container.InstantiatePrefab(prefab).GetComponent<EnemyMonster>();
        CurrentMonster.OnDied += SpawnMonster;
    }
}
