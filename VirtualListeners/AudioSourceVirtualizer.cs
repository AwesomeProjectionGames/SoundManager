using AwesomeProjectionCoreUtils.Extensions;
using UnityEngine;

namespace SoundManager.VirtualListeners
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceVirtualizer : MonoBehaviour
    {
        private AudioSource _originalSource;
        private AudioSource _proxySource;
        private Transform _proxyTransform;
    
        // State tracking to detect changes
        private bool _wasPlaying;

        private void Awake()
        {
            _originalSource = GetComponent<AudioSource>();
            CreateShadowProxy();
        }

        private void CreateShadowProxy()
        {
            if (VirtualAudioManager.Instance == null) return;

            // Create a ghost object in the Reference World
            GameObject ghost = new GameObject($"Shadow_{gameObject.name}");
            _proxyTransform = ghost.transform;
        
            // Parent to the Void anchor so it moves relative to the listener correctly
            Transform voidAnchor = VirtualAudioManager.Instance.transform;
            _proxyTransform.SetParent(voidAnchor);

            // Add the mirror AudioSource
            _proxySource = ghost.AddComponent<AudioSource>();
            _proxySource.playOnAwake = false;
        }

        private void Update()
        {
            if (_proxySource == null || VirtualAudioManager.Instance == null) return;

            // We force the original to be muted so it processes logic (time, loop) but emits no sound.
            if (!_originalSource.mute) _originalSource.mute = true;
            SyncAudioProperties();
            PositionProxy();
            HandlePlaybackState();
        }

        private void PositionProxy()
        {
            var closestListener = VirtualAudioManager.Instance.GetClosestListener(transform.position);
        
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

        private void HandlePlaybackState()
        {
            // Detect if the original started or stopped playing
            bool isOriginalPlaying = _originalSource.isPlaying;

            if (isOriginalPlaying && !_wasPlaying)
            {
                // Original just started
                _proxySource.time = _originalSource.time; // Sync playhead
                _proxySource.Play();
            }
            else if (!isOriginalPlaying && _wasPlaying)
            {
                // Original just stopped
                _proxySource.Stop();
            }
            else if (isOriginalPlaying && _wasPlaying)
            {
                // Drift Correction: If the proxy drifts too far from original time, snap it
                if (Mathf.Abs(_proxySource.time - _originalSource.time) > 0.1f)
                {
                    _proxySource.time = _originalSource.time;
                }
            }

            _wasPlaying = isOriginalPlaying;
        }

        private void OnDestroy()
        {
            if (_proxyTransform != null) Destroy(_proxyTransform.gameObject);
        }
    }
}