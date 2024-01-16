using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public abstract class CustomSlider : MonoBehaviour
    {
        protected Slider Slider;

        protected virtual void Start()
        {
            Slider = GetComponent<Slider>();
        }
    }
}