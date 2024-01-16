using DG.Tweening;
using NaughtyAttributes;
using UniRx;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CustomMenu : MonoBehaviour
    {
        [BoxGroup("Custom Menu")]
        [SerializeField] private float _animTime = 0.2f;
        [BoxGroup("Custom Menu")]
        [SerializeField] private bool _activeAtStart = false;

        public ReactiveCommand<bool> OnSwitch = new();
        protected CanvasGroup group;
        protected bool isActive;

        protected virtual void Start()
        {
            group = GetComponent<CanvasGroup>();
            
            SetActiveDefault(_activeAtStart, false);
        }

        public void SetActiveButton(bool active) => SetActive(active);

        protected virtual void SetActiveDefault(bool active, bool withAnimation = true)
        {
            isActive = active;
            group.blocksRaycasts = active;
            if (withAnimation)
            {
                group.DOFade(active ? 1 : 0, _animTime).SetUpdate(true);
            }
            else
            {
                group.alpha = active ? 1 : 0;
            }
            OnSwitch.Execute(active);
        } 
        
        public virtual void SetActive(bool active, bool withAnimation = true)
        {
            SetActiveDefault(active, withAnimation);
        }
    }
}