using UnityEngine;

namespace SoundManager.VirtualListeners
{
    /// <summary>
    /// Synchronizes an original AudioSource with a proxy AudioSource positioned relative to the real AudioListener.
    /// This allows for correct specific spatialization for multiple listeners (e.g. split-screen) by simulating
    /// the relative position of the sound source to the closest virtual listener.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceVirtualizer : VirtualAudioSourceBase
    {
        private AudioSource _originalSource;

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
                    EnsureProxy();
                    
                    // Sync playhead and state immediately
                    SyncAudioProperties();
                    _proxySource.time = _originalSource.time;
                    _proxySource.Play();
                }

                SyncAudioProperties();
                UpdateProxyPosition(isPlaying);

                // Drift Correction
                if (Mathf.Abs(_proxySource.time - _originalSource.time) > 0.1f)
                {
                    _proxySource.time = _originalSource.time;
                }
            }
            else
            {
                ReleaseProxy();
            }
            
            _wasPlaying = isPlaying;
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
    }
}
