using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager.AdaptiveMusic
{
    /// <summary>
    /// Parameters for a music intensity
    /// </summary>
    [Serializable]
    public class AdaptiveMusicIntensityParameters
    {
        /// <summary>
        /// The music loops
        /// </summary>
        [Tooltip("The music loops list for the current intensity")]
        public List<AudioClip> musicLoops;

        /// <summary>
        /// The volume overide of audio tracks
        /// </summary>
        [Tooltip("The volume overide of audio tracks")]
        public float volume = 1;

        /// <summary>
        /// The duration when the destination is the given intensity
        /// </summary>
        [Tooltip("The duration when the destination is the given intensity")]
        public float crossFadeDuration = 1;

        /// <summary>
        /// The nullable stinger sound when the destination is the given intensity
        /// </summary>
        [Tooltip("The nullable stinger sound when the destination is the given intensity")]
        public AudioClip stinger = null;

        /// <summary>
        /// Play the stinger sound at full volume or apply the volume overide ?
        /// </summary>
        [Tooltip("Play the stinger sound at full volume or apply the volume overide ?")]
        public bool playStingerFullVolume = false;

        /// <summary>
        /// Wait for the end of the stinger sound to start a new one ?
        /// </summary>
        [Tooltip("Wait for the end of the stinger sound to start a new one ?")]
        public bool waitForEndStinger = true;
    }
}