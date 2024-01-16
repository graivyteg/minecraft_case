using System;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Animator))]
public class EntityAnimator : MonoBehaviour
{
    [SerializeField] private string _movementKey = "IsMoving";
    
    private Animator _animator;
    private Entity _entity;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _entity = GetComponent<Entity>();
    }

    private void Update()
    {
        _animator.SetBool(_movementKey, _entity.IsMoving);
    }
}