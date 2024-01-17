using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioButtonSource : MonoBehaviour
    {
        public static AudioButtonSource Instance;

        [SerializeField] private AudioClip _clickClip;
        
        private AudioSource _source;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else if (Instance != this) Destroy(gameObject);

            _source = GetComponent<AudioSource>();
        }

        public void PlayClickSound()
        {
            _source.PlayOneShot(_clickClip);   
        }
    }
}