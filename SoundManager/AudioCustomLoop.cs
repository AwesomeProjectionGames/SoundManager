using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SoundManager
{
    /// <summary>
    /// This class is used to play a custom loop with an intro and an outro
    /// </summary>
    public class AudioCustomLoop : MonoBehaviour
    {
        public AudioSource source;
        public AudioClip intro;
        public AudioClip loop;
        public AudioClip end;
        public void Play()
        {
            source.loop = false;
            source.clip = intro;
            source.Play();
            StartCoroutine(PlayLoopDelayed());
        }
        IEnumerator PlayLoopDelayed()
        {
            yield return new WaitForSecondsRealtime(intro.length);
            source.clip = loop;
            source.loop = true;
            source.Play();
        }
        public void Stop()
        {
            source.loop = false;
            source.clip = end;
            source.Play();
        }
    }
}