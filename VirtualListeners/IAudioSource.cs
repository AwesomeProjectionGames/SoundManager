using UnityEngine;
using UnityEngine.Audio;

namespace SoundManager.VirtualListeners
{
    /// <summary>
    /// Defines a common interface for both native Unity AudioSources and Virtual AudioSources.
    /// </summary>
    public interface IAudioSource
    {
        /// <summary>The primary AudioClip to play.</summary>
        AudioClip Clip { get; set; }
        /// <summary>The volume level (0 to 1).</summary>
        float Volume { get; set; }
        /// <summary>The playback pitch.</summary>
        float Pitch { get; set; }
        /// <summary>Sets how much the 3D engine affects the source (0 = 2D, 1 = 3D).</summary>
        float SpatialBlend { get; set; }
        /// <summary>Should the clip cycle back to the start when finished?</summary>
        bool Loop { get; set; }
        /// <summary>Playback position in seconds.</summary>
        float Time { get; set; }
        /// <summary>Target AudioMixerGroup for routing.</summary>
        AudioMixerGroup OutputAudioMixerGroup { get; set; }
        
        /// <summary>Is the audio source currently playing?</summary>
        bool IsPlaying { get; }
        /// <summary>The GameObject this source is attached to.</summary>
        GameObject GameObject { get; }
        /// <summary>The Transform of the source for spatial positioning.</summary>
        Transform Transform { get; }

        /// <summary>Starts playing the scheduled clip.</summary>
        void Play();
        /// <summary>Stops playback immediately.</summary>
        void Stop();
        /// <summary>Pauses playback.</summary>
        void Pause();
        /// <summary>Unpauses playback.</summary>
        void UnPause();
        /// <summary>Plays a clip once without interrupting the main clip.</summary>
        /// <param name="shotClip">The clip to play.</param>
        /// <param name="volumeScale">The scale of the volume (0 to 1).</param>
        void PlayOneShot(AudioClip shotClip, float volumeScale = 1f);
    }
}