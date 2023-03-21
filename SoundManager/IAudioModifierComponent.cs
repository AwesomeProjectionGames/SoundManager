using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SoundManager
{
    /// <summary>
    /// All the childs of the audio modifier must implement this interface
    /// </summary>
    [System.Serializable]
    public abstract class IAudioModifierComponent
    {
        [SerializeField] protected bool enabled = false;
        abstract public void ModifyAudio(AudioSource audioSource);
    }
}