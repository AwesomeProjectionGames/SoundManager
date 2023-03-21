using SoundManager.AdaptiveMusic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager.AdaptiveMusic {
    /// <summary>
    /// Scriptable object that contains all the data for an adaptive music mix
    /// </summary>
    [CreateAssetMenu(fileName = "AdaptiveMusic", menuName = "SoundManager/AdaptiveMusic", order = 1)]
    public class AdaptiveMusic : ScriptableObject
    {
        // List of audio clips for different intensity levels
        [Tooltip("List of audio clips for different intensity levels")]
        public List<AdaptiveMusicIntensityParameters> intensityParameters;

        /// <summary>
        /// The cross fade duration when stopping music
        /// </summary>
        [Tooltip("The cross fade duration when stopping music")]
        public float stopFadeDuration = 1;
    }
}