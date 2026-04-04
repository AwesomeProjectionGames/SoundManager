using UnityEngine;
using UnityEngine.Audio;

namespace SoundManager.VirtualListeners
{
    /// <summary>
    /// Adapter class that wraps a standard Unity AudioSource to implement IAudioSource.
    /// </summary>
    public class AudioSourceAdapter : IAudioSource
    {
        private readonly AudioSource _source;

        /// <summary>
        /// Initializes a new instance of the adapter.
        /// </summary>
        /// <param name="source">The native Unity AudioSource to wrap.</param>
        public AudioSourceAdapter(AudioSource source)
        {
            _source = source;
        }

        public AudioClip Clip { get => _source.clip; set => _source.clip = value; }
        public float Volume { get => _source.volume; set => _source.volume = value; }
        public float Pitch { get => _source.pitch; set => _source.pitch = value; }
        public float SpatialBlend { get => _source.spatialBlend; set => _source.spatialBlend = value; }
        public bool Loop { get => _source.loop; set => _source.loop = value; }
        public float Time { get => _source.time; set => _source.time = value; }
        public AudioMixerGroup OutputAudioMixerGroup { get => _source.outputAudioMixerGroup; set => _source.outputAudioMixerGroup = value; }

        public bool IsPlaying => _source.isPlaying;
        public GameObject GameObject => _source.gameObject;
        public Transform Transform => _source.transform;

        public void Play() => _source.Play();
        public void Stop() => _source.Stop();
        public void Pause() => _source.Pause();
        public void UnPause() => _source.UnPause();
        public void PlayOneShot(AudioClip shotClip, float volumeScale = 1f) => _source.PlayOneShot(shotClip, volumeScale);
    }
}