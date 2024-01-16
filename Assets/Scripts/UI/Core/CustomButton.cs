using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class CustomButton : MonoBehaviour
    {
        protected Button Button;

        protected virtual void Start()
        {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(OnClick);
        }

        protected virtual void Update()
        {
            Button.interactable = IsInteractable();
        }

        protected virtual bool IsInteractable()
        {
            return Button.interactable;
        }

        protected abstract void OnClick();
    }
}