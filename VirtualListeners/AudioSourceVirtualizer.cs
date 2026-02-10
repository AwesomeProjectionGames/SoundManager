using AwesomeProjectionCoreUtils.Extensions;
using UnityEngine;

namespace SoundManager.VirtualListeners
{
    /// <summary>
    /// Synchronizes an original AudioSource with a proxy AudioSource positioned relative to the real AudioListener.
    /// This allows for correct specific spatialization for multiple listeners (e.g. split-screen) by simulating
    /// the relative position of the sound source to the closest virtual listener.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceVirtualizer : MonoBehaviour
    {
        private AudioSource _originalSource;
        private AudioSource _proxySource;
        private Transform _proxyTransform;

        [Tooltip("If true, the closest listener is constantly updated. If false, it is locked when playback starts.")]
        public bool updateListenerWhilePlaying = false;
        private AudioListenerVirtual _cachedListener;
    
        private bool _wasPlaying; // Kept for logic if needed, but primary logic is now state-based

        private void Awake()
        {
            _originalSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (VirtualAudioManager.Instance == null) return;

            // We force the original to be muted so it processes logic (time, loop) but emits no sound.
            if (!_originalSource.mute) _originalSource.mute = true;

            bool isPlaying = _originalSource.isPlaying;

            if (isPlaying)
            {
                if (_proxySource == null)
                {
                    _proxySource = VirtualAudioManager.Instance.GetProxySource();
                    _proxyTransform = _proxySource.transform;
                    
                    // Sync playhead and state immediately
                    SyncAudioProperties();
                    _proxySource.time = _originalSource.time;
                    _proxySource.Play();
                }

                SyncAudioProperties();
                PositionProxy();

                // Drift Correction
                if (Mathf.Abs(_proxySource.time - _originalSource.time) > 0.1f)
                {
                    _proxySource.time = _originalSource.time;
                }
            }
            else
            {
                if (_proxySource != null)
                {
                    VirtualAudioManager.Instance.ReturnProxySource(_proxySource);
                    _proxySource = null;
                    _proxyTransform = null;
                }
            }
            
            _wasPlaying = isPlaying;
        }

        private void PositionProxy()
        {
            AudioListenerVirtual closestListener;

            if (!updateListenerWhilePlaying && _originalSource.isPlaying)
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
                // Calculate relative offset from the virtual ear
                Vector3 relativePos = closestListener!.transform.InverseTransformPoint(transform.position);
            
                // Apply that offset to the Real Listener in the Void
                Transform voidAnchor = VirtualAudioManager.Instance.transform;
                _proxyTransform.position = voidAnchor.TransformPoint(relativePos);
            }
            else
            {
                _proxyTransform.localPosition = new Vector3(0, 0, 0);
            }
        }

        private void SyncAudioProperties()
        {
            // Copy volume/pitch/clip/settings continuously so designer tweaks work in Play Mode
            _proxySource.volume = _originalSource.volume; // We copy the value, even though original is muted
            _proxySource.pitch = _originalSource.pitch;
            _proxySource.spatialBlend = _originalSource.spatialBlend;
            _proxySource.minDistance = _originalSource.minDistance;
            _proxySource.maxDistance = _originalSource.maxDistance;
            _proxySource.rolloffMode = _originalSource.rolloffMode;
            _proxySource.dopplerLevel = _originalSource.dopplerLevel;
            _proxySource.loop = _originalSource.loop;
            _proxySource.outputAudioMixerGroup = _originalSource.outputAudioMixerGroup;
        
            // Only swap clip if changed (optimization)
            if (_proxySource.clip != _originalSource.clip)
                _proxySource.clip = _originalSource.clip;
        }

        private void OnDisable()
        {
            if (_proxySource != null)
            {
                // Use ?. in case VirtualAudioManager is already destroyed (e.g. app quit)
                VirtualAudioManager.Instance?.ReturnProxySource(_proxySource);
                _proxySource = null;
                _proxyTransform = null;
            }
        }

        private void OnDestroy()
        {
            if (_proxySource != null)
            {
                VirtualAudioManager.Instance?.ReturnProxySource(_proxySource);
                _proxySource = null;
                _proxyTransform = null;
            }
        }
    }
}
