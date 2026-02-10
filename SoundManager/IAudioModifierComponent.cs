using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundManager.VirtualListeners;
namespace SoundManager
{
    /// <summary>
    /// All the childs of the audio modifier must implement this interface
    /// </summary>
    [System.Serializable]
    public abstract class IAudioModifierComponent
    {
        [SerializeField] protected bool enabled = false;
        public abstract void ModifyAudio(AudioSource audioSource);
        public abstract void ModifyAudio(AudioSourceVirtual audioSource);
    }
}