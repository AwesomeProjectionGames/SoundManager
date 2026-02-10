using SoundManager.VirtualListeners;
using UnityEngine;

namespace SoundManager.Modifier
{
    /// <summary>
    /// This modifier is used to apply a random pitch to the audio source
    /// </summary>
    [System.Serializable]
    public class PitchModifier : IAudioModifierComponent
    {
        [SerializeField] private float minPitch = 0.8f;
        [SerializeField] private float maxPitch = 1.2f;
        public override void ModifyAudio(AudioSource audioSource)
        {
            if (!enabled) return;
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        }
        public override void ModifyAudio(AudioSourceVirtual audioSource)
        {
            if (!enabled) return;
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        }
    }
}