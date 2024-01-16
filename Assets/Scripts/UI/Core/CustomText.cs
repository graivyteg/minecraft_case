using System;
using TMPro;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class CustomText : MonoBehaviour
    {
        protected TMP_Text Text;
        
        protected virtual void Start()
        {
            Text = GetComponent<TMP_Text>();
        }
    }
}