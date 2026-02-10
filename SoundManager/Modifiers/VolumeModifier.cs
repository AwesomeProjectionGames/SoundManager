using SoundManager.VirtualListeners;
using UnityEngine;
using UnityEngine.Serialization;

namespace SoundManager.Modifier
{
    /// <summary>
    /// This modifier is used to apply a random pitch to the audio source
    /// </summary>
    [System.Serializable]
    public class VolumeModifier : IAudioModifierComponent
    {
        [FormerlySerializedAs("minPitch")] [SerializeField] private float minVolume = 0.8f;
        [FormerlySerializedAs("maxPitch")] [SerializeField] private float maxVolume = 1.2f;
        public override void ModifyAudio(AudioSource audioSource)
        {
            if (!enabled) return;
            audioSource.volume = Random.Range(minVolume, maxVolume);
        }

        public override void ModifyAudio(AudioSourceVirtual audioSource)
        {
            if (!enabled) return;
            audioSource.volume = Random.Range(minVolume, maxVolume);
        }
    }
}