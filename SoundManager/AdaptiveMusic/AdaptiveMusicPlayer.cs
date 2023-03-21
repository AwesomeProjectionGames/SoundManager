using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoundManager.AdaptiveMusic
{
    /// <summary>
    /// This adaptive music is used to change the music according to the game status
    /// </summary>
    public class AdaptiveMusicPlayer : MonoBehaviour
    {
        // Reference to the audio source components
        [SerializeField] AudioSource audioSource;
        [SerializeField] AudioSource crossfadeAudioSource;
        [SerializeField] AudioSource stingerAudioSource;

        // The current adaptive music mix to play
        [SerializeField] AdaptiveMusic currentAdaptiveMusicMix;
        /// <summary>
        /// Get or change the current adaptive music mix to play / playing
        /// </summary>
        public AdaptiveMusic CurrentAdaptiveMusicMix
        {
            get { return currentAdaptiveMusicMix; }
            set
            {
                StartCoroutine(ChangeCurrentMix(currentAdaptiveMusicMix));
            }
        }

        // Current intensity level
        [SerializeField] int currentIntensity = 0;
        /// <summary>
        /// Get or change+play the current intensity level
        /// </summary>
        public int CurrentIntensity { get { return currentIntensity; } set { currentIntensity = value; PlayIntensityLevel(); } }

        [SerializeField] bool startAtIntensity = false;
        internal bool isInTransition = false;

        void Start()
        {
            if (startAtIntensity) PlayIntensityLevel();
        }

        /// <summary>
        /// Play or resume the corresponding intensity parameter from music
        /// </summary>
        public void PlayIntensityLevel()
        {
            if(currentAdaptiveMusicMix != null)
            {
                StartCoroutine(CrossFade(currentAdaptiveMusicMix.intensityParameters[currentIntensity]));
            }
            else
            {
                Stop();
            }
        }

        /// <summary>
        /// Stop the current music with a fade out
        /// </summary>
        public void Stop()
        {
            StartCoroutine(FadeToStop());
        }

        IEnumerator ChangeCurrentMix(AdaptiveMusic newAdaptiveMusic)
        {
            while (isInTransition) yield return null;
            currentAdaptiveMusicMix = newAdaptiveMusic;
            if(currentAdaptiveMusicMix != null)
            {
                if (currentIntensity >= newAdaptiveMusic.intensityParameters.Count)
                    currentIntensity = newAdaptiveMusic.intensityParameters.Count - 1;
            }
            else
            {
                currentIntensity = 0;
            }
            PlayIntensityLevel();
        }

        /// <summary>
        /// Fade to stop the music
        /// </summary>
        IEnumerator FadeToStop()
        {
            while (isInTransition) yield return null;
            isInTransition = true;
            if (currentAdaptiveMusicMix != null)
            {
                while (audioSource.volume > 0)
                {
                    audioSource.volume -= Time.deltaTime / currentAdaptiveMusicMix.stopFadeDuration;
                    yield return null;
                }
            }
            crossfadeAudioSource.Stop();
            audioSource.Stop();
            stingerAudioSource.Stop();
            isInTransition = false;
        }

        // Coroutine to perform cross fade between audio clips
        IEnumerator CrossFade(AdaptiveMusicIntensityParameters toIntensity)
        {
            while (isInTransition) yield return null;
            isInTransition = true;

            //Get a random new clip from the next intensity
            AudioClip newClip = toIntensity.musicLoops[Random.Range(0, toIntensity.musicLoops.Count)];

            // Calculate fade out and fade in times
            float fadeOutTime = audioSource.volume / toIntensity.crossFadeDuration;
            float fadeInTime = 1f / toIntensity.crossFadeDuration;

            // Play stinger sound if any
            if(toIntensity.stinger != null)
            {
                stingerAudioSource.clip = toIntensity.stinger;
                stingerAudioSource.Play();
                if (!toIntensity.playStingerFullVolume) stingerAudioSource.volume = toIntensity.volume;
                if (toIntensity.waitForEndStinger) yield return new WaitForSeconds(toIntensity.stinger.length);
            }

            // Set new clip and play it on the new audio source
            crossfadeAudioSource.clip = newClip;
            crossfadeAudioSource.volume = 0;
            crossfadeAudioSource.Play();

            // Fade out current clip and fade in new clip
            while (audioSource.volume > 0 || crossfadeAudioSource.volume < toIntensity.volume)
            {
                audioSource.volume -= fadeOutTime * Time.deltaTime;
                crossfadeAudioSource.volume += fadeInTime * Time.deltaTime * toIntensity.volume;
                yield return null;
            }

            // Set new audio source as the active audio source and transfer the current time
            AudioSource newOfficialSource = crossfadeAudioSource;
            crossfadeAudioSource = audioSource;
            audioSource = newOfficialSource;
            crossfadeAudioSource.Stop();
            isInTransition = false;
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(AdaptiveMusicPlayer))]
    public class AdaptiveMusicPlayerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);
            GUILayout.Label("Test (playmode) :", EditorStyles.boldLabel);

            AdaptiveMusicPlayer myScript = (AdaptiveMusicPlayer)target;
            GUILayout.Label("Intensity "+ myScript.CurrentIntensity + " :");
            if (GUILayout.Button("+"))
            {
                myScript.CurrentIntensity++;
            }
            if (GUILayout.Button("-"))
            {
                myScript.CurrentIntensity--;
            }

            GUILayout.Label("Player : ");
            if (GUILayout.Button("Start"))
            {
                myScript.PlayIntensityLevel();
            }
            if (GUILayout.Button("Stop"))
            {
                myScript.Stop();
            }
        }
    }
#endif
}