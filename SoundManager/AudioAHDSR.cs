using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SoundManager
{
    /// <summary>
    /// A class that modifies the audio source (fading in / out)
    /// </summary>
    public class AudioAHDSR : MonoBehaviour
    {
        [Tooltip("The audio source to modify")]
        [SerializeField] private AudioSource audioSource;
        [Tooltip("Fade in duration in seconds (0 = no fade)")]
        [SerializeField] private float fadInTime = 1;
        [Tooltip("Start fade in at start ? Or fade in manually with method")]
        [SerializeField] private bool fadeInAtStart = true;
        [Tooltip("On fade in (finished delay)")]
        [SerializeField] private UnityEvent onFadeIn;
        [Tooltip("Fade in duration in seconds (0 = no fade)")]
        [SerializeField] private float fadOutTime = 1;
        [Tooltip("On fade out (finished delay)")]
        [SerializeField] private UnityEvent onFadeOut;
        [Tooltip("Let the audio active for fade out on a new scene")]
        [SerializeField] private bool dontDestroyOnLoadForFadeOut = true;
        [Tooltip("Stop the audiosource after faded out")]
        [SerializeField] private bool stopAfterFadeOut = true;

        [SerializeField]
        private bool isFading = false;
        [SerializeField]
        private bool fadeInWhenEnabled = false;
        void Start()
        {
            if (fadeInAtStart)
            {
                FadeIn();
            }
            if (dontDestroyOnLoadForFadeOut)
            {
                DontDestroyOnLoad(gameObject);
                SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
            }
        }

        private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
        {
            FadeOut(true);
        }

        void OnEnable()
        {
            if (fadeInWhenEnabled)
            {
                fadeInWhenEnabled = false;
                FadeIn();
            }
        }

        /// <summary>
        /// Start fade in
        /// </summary>
        public void FadeIn()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(FadeInCoroutine());
            else
                fadeInWhenEnabled = true;
        }
        /// <summary>
        /// Start fade out
        /// </summary>
        public void FadeOut(bool destroyAfter = false)
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(FadeOutCoroutine(destroyAfter));
        }

        IEnumerator FadeInCoroutine()
        {
            yield return new WaitUntil(() => !isFading);
            float time = 0;
            float startVolume = 0;
            float endVolume = audioSource.volume;
            isFading = true;
            while (time < fadInTime)
            {
                time += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, endVolume, time / fadInTime);
                yield return null;
            }
            audioSource.volume = endVolume;
            isFading = false;
            onFadeIn?.Invoke();
        }
        IEnumerator FadeOutCoroutine(bool destroyAfter)
        {
            yield return new WaitUntil(() => !isFading);
            float time = 0;
            float startVolume = audioSource.volume;
            float endVolume = 0;
            isFading = true;
            while (time < fadOutTime)
            {
                time += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, endVolume, time / fadOutTime);
                yield return null;
            }
            audioSource.volume = endVolume;
            isFading = false;
            onFadeOut?.Invoke();
            if (destroyAfter)
            {
                Destroy(gameObject);
            }
            if (stopAfterFadeOut)
            {
                audioSource.Stop();
            }
        }
    }
}