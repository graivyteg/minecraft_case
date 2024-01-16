using System;
using System.Collections;
using NaughtyAttributes;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using YG;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Entity : MonoBehaviour
{
    public EntityData Data { get; private set; }
    
    public EntityLevelData LevelData { get; private set; }
    public bool IsMoving { get; private set; }

    [SerializeField] private float _idleTime = 1;
    
    [Foldout("Advanced")] 
    [SerializeField] private float _walkRadius = 3;
    [Foldout("Advanced")]
    [SerializeField] private float _failedMovePointDelay = 0.5f;
    
    private NavMeshAgent _agent;

    private Action<Vector3> OnMovePointUpdate;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        IsMoving = false;
    }

    private void Start()
    {
        NavMesh.SamplePosition(transform.position, out var hit, 100f, NavMesh.AllAreas);
        _agent.Warp(hit.position);

        StartCoroutine(CalculateMovePoint());
    }

    public void InitializeData(EntityData data)
    {
        Data = data;
        var index = EntityUtil.GetSaveIndex(data.Key);
        if (index == -1)
        {
            YandexGame.savesData.entities.Add(new EntityLevelData(data.Key));
            index = YandexGame.savesData.entities.Count - 1;
        } 
        
        LevelData = YandexGame.savesData.entities[index];
    }

    private void Update()
    {
        var distanceToPoint = Vector3.Distance(transform.position, _agent.destination);
        
        if (IsMoving && distanceToPoint < 0.1f)
        {
            IsMoving = false;
            Observable.Timer(TimeSpan.FromSeconds(_idleTime)).Subscribe(_ =>
            {
                StartCoroutine(CalculateMovePoint());
            });
        }
    }

    private IEnumerator CalculateMovePoint()
    {
        Vector3 point;
        while (true)
        {
            point = GetRandomPoint();
            var path = new NavMeshPath();
            _agent.CalculatePath(point, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                break;
            }

            yield return new WaitForSeconds(_failedMovePointDelay);
        }
        
        OnMovePointCalculated(point);
    }

    private void OnMovePointCalculated(Vector3 point)
    {
        _agent.destination = point;
        IsMoving = true;
    }
    
    private Vector3 GetRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _walkRadius;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, _walkRadius, 1);
        return hit.position;
    }
    
    public int GetUpgradePrice()
    {
        return Data.FirstUpgradePrice * Mathf.RoundToInt(Mathf.Pow(2, LevelData.Level - 1));
    }

    public int GetDamage()
    {
        return Data.DefaultDamage + Data.DamagePerLevel * (LevelData.Level - 1);
    }
}