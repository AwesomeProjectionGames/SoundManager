using AwesomeProjectionCoreUtils.Extensions;
using UnityEngine;

namespace SoundManager.VirtualListeners
{
    public abstract class VirtualAudioSourceBase : MonoBehaviour
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
    }
}

