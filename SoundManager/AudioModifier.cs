using SoundManager.Modifier;
using SoundManager.VirtualListeners;
using UnityEngine;

namespace SoundManager
{
    /// <summary>
    /// A class that modifies the audio source based on the AudioModifierComponents
    /// </summary>
    public class AudioModifier : MonoBehaviour
    {
        [Tooltip("The audio source to modify")]
        [SerializeField] private AudioSource audioSource;
        [Tooltip("The virtual audio source to modify")]
        [SerializeField] private AudioSourceVirtual audioSourceVirtual;
        [Tooltip("Modify the audio source eatch time the sound is played ? (Or only once at awake)")]
        [SerializeField] private bool modifyWhenPlay = true;
        [Tooltip("The pitch modifier of the audio source")]
        [SerializeField] private PitchModifier pitchModifier;
        [Tooltip("The volume modifier of the audio source")]
        [SerializeField] private VolumeModifier volumeModifier;
        [Tooltip("The random clip selector modifier of the audio source")]
        [SerializeField] private RandomClipModifier randomClipModifier;
        [Tooltip("The random offset modifier of the audio source")]
        [SerializeField] private OffsetSoundModifier offsetModifier;
        private IAudioModifierComponent[] audioModifierComponents;
        /// <summary>
        /// Get the audio source direct reference
        /// </summary>
        public AudioSource AudioSource => audioSource;
        /// <summary>
        /// Get the virtual audio source direct reference
        /// </summary>
        public AudioSourceVirtual AudioSourceVirtual => audioSourceVirtual;

        private void Awake()
        {
            if (audioSource == null && audioSourceVirtual == null) audioSource = GetComponent<AudioSource>();
            if (audioSource == null && audioSourceVirtual == null) audioSourceVirtual = GetComponent<AudioSourceVirtual>();

            if (!modifyWhenPlay)
                ModifyAudio();
        }
        /// <summary>
        /// Modify and play the audio source
        /// </summary>
        public void Play()
        {
            if (modifyWhenPlay)
                ModifyAudio();
            if (audioSource != null) audioSource.Play();
            if (audioSourceVirtual != null) audioSourceVirtual.Play();
        }
        /// <summary>
        /// Modify the audio source based on the modifier components
        /// </summary>
        public void ModifyAudio()
        {
            if (audioModifierComponents == null)
                audioModifierComponents = new IAudioModifierComponent[] { pitchModifier, volumeModifier, randomClipModifier, offsetModifier };
            foreach (var audioModifierComponent in audioModifierComponents)
            {
                if (audioSource != null) audioModifierComponent.ModifyAudio(audioSource);
                if (audioSourceVirtual != null) audioModifierComponent.ModifyAudio(audioSourceVirtual);
            }
        }
    }
}