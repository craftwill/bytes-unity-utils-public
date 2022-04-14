using System.Collections;
using UnityEngine;

namespace Bytes.Sound
{
    [CreateAssetMenu(fileName = "Sound", menuName = "SoundTemplate")]
    public class SoundTemplate : ScriptableObject
    {
        [Header("Text")]
        public new string name;

        [Header("Sound Data")]
        [Tooltip("Sound variations if many.")]
        public AudioClip[] clips;
        public float volume = 1f;
        [Tooltip("Max Random pitch change.")]
        public float maxRandomPitchChange = 0.02f;

        public AudioClip GetRandomClip()
        {
            return clips[Random.Range(0, clips.Length)];
        }

        public float GetRandomPitch()
        {
            return 1f + Random.Range(-maxRandomPitchChange, maxRandomPitchChange);
        }
    }
}