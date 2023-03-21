using UnityEngine;

namespace SoundManager.Modifier
{
    /// <summary>
    /// This modifier is used to start the audio source at a random time
    /// </summary>
    [System.Serializable]
    public class OffsetSoundModifier : IAudioModifierComponent
    {
        [SerializeField] private float minOffset = 0f;
        [SerializeField] private float maxOffset = 1f;
        public override void ModifyAudio(AudioSource audioSource)
        {
            if (!enabled) return;
            audioSource.time = Random.Range(minOffset, maxOffset);
        }
    }
}