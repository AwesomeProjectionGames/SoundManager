using UnityEngine;

namespace SoundManager.Modifier
{
    /// <summary>
    /// This modifier is used to apply a random clip to the audio source
    /// </summary>
    [System.Serializable]
    public class RandomClipModifier : IAudioModifierComponent
    {
        [SerializeField] private AudioClip[] clips;
        public override void ModifyAudio(AudioSource audioSource)
        {
            if (!enabled) return;
            audioSource.clip = clips[Random.Range(0, clips.Length)];
        }
    }
}