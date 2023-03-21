using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SoundManager
{
    /// <summary>
    /// This is used to manage a custom loop with custom start and end
    /// </summary>
    public class AudioLoopStartEnd : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip start;
        [SerializeField]
        private AudioClip loop;
        [SerializeField]
        private AudioClip end;
        /// <summary>
        /// Start the audio source with the start clip
        /// </summary>
        public void Play()
        {
            StartCoroutine(PlayLoop());
        }
        /// <summary>
        /// Stop the loop and play the end clip
        /// </summary>
        public void Stop()
        {
            StopAllCoroutines();
            if (end != null)
            {
                audioSource.clip = end;
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
        private IEnumerator PlayLoop()
        {
            if (start != null)
            {
                audioSource.clip = start;
                audioSource.Play();
                yield return new WaitForSeconds(start.length);
            }
            audioSource.clip = loop;
            audioSource.Play();
        }
    }
}