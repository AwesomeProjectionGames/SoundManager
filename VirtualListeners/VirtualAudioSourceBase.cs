using AwesomeProjectionCoreUtils.Extensions;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundManager.VirtualListeners
{
    public abstract class VirtualAudioSourceBase : MonoBehaviour, IAudioSource
    {
        [Tooltip("If true, the closest listener is constantly updated. If false, it is locked when playback starts.")]
        public bool updateListenerWhilePlaying;

        protected AudioSource _proxySource;
        protected Transform _proxyTransform;
        protected AudioListenerVirtual _cachedListener;
        protected bool _wasPlaying;

        protected virtual void OnDisable()
        {
            ReleaseProxy();
        }

        protected virtual void OnDestroy()
        {
            ReleaseProxy();
        }

        protected void EnsureProxy()
        {
            if (_proxySource == null && VirtualAudioManager.Instance != null)
            {
                _proxySource = VirtualAudioManager.Instance.GetProxySource();
                _proxyTransform = _proxySource.transform;
            }
        }

        protected void ReleaseProxy()
        {
            if (_proxySource != null)
            {
                VirtualAudioManager.Instance?.ReturnProxySource(_proxySource);
                _proxySource = null;
                _proxyTransform = null;
            }
        }

        protected void UpdateProxyPosition(bool isPlaying)
        {
            if (_proxyTransform == null || VirtualAudioManager.Instance == null) return;

            AudioListenerVirtual closestListener;

            if (!updateListenerWhilePlaying && isPlaying)
            {
                if (!_wasPlaying || _cachedListener == null)
                {
                    _cachedListener = VirtualAudioManager.Instance.GetClosestListener(transform.position);
                }
                closestListener = _cachedListener;
            }
            else
            {
                closestListener = VirtualAudioManager.Instance.GetClosestListener(transform.position);
            }
        
            if (closestListener.IsAlive())
            {
                Vector3 relativePos = closestListener!.transform.InverseTransformPoint(transform.position);
                Transform voidAnchor = VirtualAudioManager.Instance.transform;
                _proxyTransform.position = voidAnchor.TransformPoint(relativePos);
            }
            else
            {
                _proxyTransform.localPosition = Vector3.zero;
            }
        }

        public abstract AudioClip Clip { get; set; }
        public abstract float Volume { get; set; }
        public abstract float Pitch { get; set; }
        public abstract float SpatialBlend { get; set; }
        public abstract bool Loop { get; set; }
        public abstract float Time { get; set; }
        public abstract AudioMixerGroup OutputAudioMixerGroup { get; set; }
        public abstract bool IsPlaying { get; }
        public abstract GameObject GameObject { get; }
        public abstract Transform Transform { get; }
        public abstract void Play();
        public abstract void Stop();
        public abstract void Pause();
        public abstract void UnPause();
        public abstract void PlayOneShot(AudioClip shotClip, float volumeScale = 1);
    }
}

