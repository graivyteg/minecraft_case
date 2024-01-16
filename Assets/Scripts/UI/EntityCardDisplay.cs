using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EntityCardDisplay : MonoBehaviour
    {
        public EntityData Data { get; private set; }
        
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _icon;

        public void Initialize(EntityData data)
        {
            Data = data;
            _name.text = data.Name;
            _icon.sprite = data.Icon;
        }
    }
}