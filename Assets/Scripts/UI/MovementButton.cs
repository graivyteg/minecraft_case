using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MovementButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private bool _isRight = false;
        private bool _isPressed = false;
        private CameraMovement _camera;

        private void Start()
        {
            _camera = Camera.main.GetComponent<CameraMovement>();
        }

        private void Update()
        {
            var value = _isRight ? 1 : -1;
            value = _isPressed ? value : 0;
            if (value != 0)
            {
                _camera.SetMovingButton(value);   
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isPressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPressed = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPressed = false;
        }
    }
}