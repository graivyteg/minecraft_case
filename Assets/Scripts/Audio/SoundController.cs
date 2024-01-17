using System;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundController : MonoBehaviour
    {
        
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void Play(AudioClip clip)
        {
            _source.PlayOneShot(clip);
        }
    }
}