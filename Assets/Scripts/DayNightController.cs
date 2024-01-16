using System;
using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [SerializeField] private float _dayLength = 90;
    
    private Material _skybox;
    private Light _sun;

    private float _timer;
    
    private void Start()
    {
        _skybox = RenderSettings.skybox;
        _sun = RenderSettings.sun;
    }

    private void Update()
    {
       
        _timer += Mathf.Min(Time.deltaTime, _dayLength - _timer);

        if (_timer >= _dayLength)
        {
            _timer = 0;
        }
        
        _sun.transform.rotation = Quaternion.Euler(_timer / _dayLength * 360 + 90, 45, 0);
        _skybox.SetFloat("_CubemapTransition", 1 - Mathf.Abs(2 * _timer / _dayLength - 1));
    }
}
