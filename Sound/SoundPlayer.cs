using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundTemplate[] playableSounds;
        private Dictionary<string, SoundTemplate> _playableSounds;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _playableSounds = new Dictionary<string, SoundTemplate>();

            foreach (var sound in playableSounds)
            {
                _playableSounds.Add(sound.name, sound);
            }
        }

        public void PlaySound(string soundName, float volumeMultiplier = 1f, float overrideVolume = -1f)
        {
            if (_playableSounds.TryGetValue(soundName, out SoundTemplate sound))
            {
                _audioSource.clip = sound.GetRandomClip();
                _audioSource.pitch = sound.GetRandomPitch();
                _audioSource.volume = ((overrideVolume != -1f) ? overrideVolume : sound.volume) * volumeMultiplier;
                _audioSource.Play();
            }
        }
    }
}