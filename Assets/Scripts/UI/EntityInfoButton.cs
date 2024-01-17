using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Canvas))]
    public class EntityInfoButton : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Button _button;
        [SerializeField] private Image _upgradeAvailable;
        
        private EntityInfoPopup _popup;
        private Entity _entity;

        public void Initialize(EntityInfoPopup popup)
        {
            _popup = popup;
        }
        
        protected void Start()
        {
            _entity = GetComponentInParent<Entity>();
            GetComponent<Canvas>().worldCamera = Camera.main;
            
            _icon.sprite = _entity.Data.Icon;
            _button.onClick.AddListener(OnClick);
        }

        protected void Update()
        {
            transform.LookAt(Camera.main.transform);

            _upgradeAvailable.gameObject.SetActive(_entity.LevelData.CanUpgrade());
        }

        protected void OnClick()
        {
            _popup.Show(_entity);
        }
    }
}