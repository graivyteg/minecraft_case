using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _firstPoint;
    [SerializeField] private Transform _secondPoint;
    
    [BoxGroup("Auto Movement")]
    [SerializeField] private float _autoSpeed = 0.2f;

    [BoxGroup("Manual Movement")] 
    [SerializeField] private float _manualSpeed = 0.75f;
    [BoxGroup("Manual Movement")] 
    [SerializeField] private float _autoDelay = 3;

    private float _normalizedPosition = 0;
    private float _manualMovementTimer = 0;
    private float _lerpFactor = 0.2f;
    private bool _invert = false;
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Move(true);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Move(false);
        }

        _manualMovementTimer -= Mathf.Min(_manualMovementTimer, Time.deltaTime);
        
        if (_manualMovementTimer == 0)
        {
            if (_normalizedPosition >= 0.99f && !_invert || 
                _normalizedPosition <= 0.01f && _invert)
            {
                _invert = !_invert;
            }

            var deltaNormalizedPosition = _autoSpeed * Time.deltaTime;
            if (_invert) deltaNormalizedPosition *= -1;

            _normalizedPosition = Mathf.Clamp01(_normalizedPosition + deltaNormalizedPosition);
        }

        var targetPosition = Vector3.Lerp(_firstPoint.position, _secondPoint.position, _normalizedPosition);

        transform.position = Vector3.Lerp(transform.position, targetPosition, _lerpFactor);
    }

    public void Move(bool right)
    {
        _manualMovementTimer = _autoDelay;
        
        var deltaNormalizedPosition = _manualSpeed * Time.deltaTime;
        if (!right)
        {
            deltaNormalizedPosition *= -1;
        }
        
        _normalizedPosition = Mathf.Clamp01(_normalizedPosition + deltaNormalizedPosition);
    }
}