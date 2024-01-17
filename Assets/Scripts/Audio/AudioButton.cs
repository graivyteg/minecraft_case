using Audio;
using UI.Buttons;
using Zenject;

namespace Audio
{
    public class AudioButton : CustomButton
    {
        protected override void OnClick()
        {
            AudioButtonSource.Instance.PlayClickSound();
        }
    }
}