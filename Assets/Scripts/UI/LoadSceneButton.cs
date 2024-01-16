using UI.Buttons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LoadSceneButton : CustomButton
    {
        [SerializeField] private int _buildIndex;
        
        protected override void OnClick()
        {
            SceneManager.LoadScene(_buildIndex);
        }
    }
}