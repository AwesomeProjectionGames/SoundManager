using UnityEngine;
using UnityEngine.Audio;

namespace SoundManager.VirtualListeners
{
    /// <summary>
    /// A virtual AudioSource which replaces the standard AudioSource but uses the VirtualAudioManager system
    /// to spatialize playbacks relative to the closest listener.
    /// </summary>
    public class AudioSourceVirtual : VirtualAudioSourceBase
    {
        [Header("Audio Settings")]
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(-3f, 3f)] public float pitch = 1f;
        [Range(0f, 1f)] public float spatialBlend;
        public bool loop;
        public AudioMixerGroup outputAudioMixerGroup;
        public float minDistance = 1f;
        public float maxDistance = 500f;
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;
        public float dopplerLevel = 1f;
        
        public bool playOnAwake = true;

        private float _lastOneShotEndTime = -1f;

        private void Start()
        {
            if (playOnAwake && clip != null)
            {
                Play();
            }
        }

        private void Update()
        {
            if (VirtualAudioManager.Instance == null) return;

            // Determine if we should be active
            bool isProxyPlaying = _proxySource != null && _proxySource.isPlaying;
            bool isOneShotActive = Time.time < _lastOneShotEndTime;
            bool shouldBeActive = isProxyPlaying || isOneShotActive;

            // However, note that _proxySource.isPlaying turns false when clip ends (if not looping).
            // But if we haven't assigned a clip (only OneShot), isPlaying is false.
            // If we assigned a clip and Play(), isPlaying is true until end.
            
            if (shouldBeActive)
            {
                EnsureProxy();
                SyncAudioProperties();
                UpdateProxyPosition(shouldBeActive);
            }
            else
            {
                ReleaseProxy();
            }
            
            _wasPlaying = shouldBeActive;
        }

        public void Play()
        {
            EnsureProxy();
            SyncAudioProperties();
            
            if (_proxySource.clip != clip) _proxySource.clip = clip;
            
            _proxySource.Play();
        }

        public void Stop()
        {
            if (_proxySource != null)
            {
                _proxySource.Stop();
            }
            ReleaseProxy();
            _lastOneShotEndTime = -1f; // Cancel OneShot wait logic
        }

        public void PlayOneShot(AudioClip shotClip, float volumeScale = 1f)
        {
            if (shotClip == null) return;

            EnsureProxy();
            SyncAudioProperties();
            UpdateProxyPosition(true); // Ensure position is correct before playing

            _proxySource.PlayOneShot(shotClip, volumeScale);

            // Estimate duration to keep proxy alive
            // We use current pitch. If pitch changes, this might be inaccurate.
            float currentPitch = Mathf.Abs(pitch) < 0.01f ? 1f : Mathf.Abs(pitch);
            float duration = shotClip.length / currentPitch;
            
            // Extend the active window
            float endTime = Time.time + duration;
            if (endTime > _lastOneShotEndTime)
            {
                _lastOneShotEndTime = endTime;
            }
        }

        private void SyncAudioProperties()
        {
            if (_proxySource == null) return;

            _proxySource.volume = volume;
            _proxySource.pitch = pitch;
            _proxySource.spatialBlend = spatialBlend;
            _proxySource.minDistance = minDistance;
            _proxySource.maxDistance = maxDistance;
            _proxySource.rolloffMode = rolloffMode;
            _proxySource.dopplerLevel = dopplerLevel;
            _proxySource.loop = loop;
            _proxySource.outputAudioMixerGroup = outputAudioMixerGroup;

            // For the main clip, we only set it if Play() logic handles it?
            // If I change 'clip' in inspector while playing, should it update?
            // AudioSource behavior: Changing the clip while playing doesn't restart it, but does it swap?
            // Unity AudioSource: assigning clip stops playback usually? Or just changes property?
            // Let's stick to safe sync:
            if (_proxySource.clip != clip)
            {
                // If we are playing the main clip, swapping might be disruptive.
                // But the user might expect it.
                // However, the proxy might be playing a OneShot. Swapping clip property on proxy is safe for OneShot?
                // PlayOneShot doesn't depend on .clip property.
                _proxySource.clip = clip;
            }
        }
    }
}

