using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Inject] private DiContainer _container;
    
    [SerializeField] private List<EnemyMonster> _enemyPrefabs;

    public EnemyMonster CurrentMonster { get; private set; }
    
    private void Start()
    {
        SpawnMonster();
    }

    private void SpawnMonster()
    {
        var prefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
        CurrentMonster = _container.InstantiatePrefab(prefab).GetComponent<EnemyMonster>();
        CurrentMonster.OnDied += SpawnMonster;
    }
}
