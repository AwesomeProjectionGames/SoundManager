using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoundManager
{
    [CreateAssetMenu(fileName = "Playlist", menuName = "SoundManager/AudioRadioPlaylist")]
    public class AudioRadioPlaylist : ScriptableObject
    {
        /// <summary>
        /// A unique identifier for this playlist (set of songs)
        /// </summary>
        public int playlistID;
        /// <summary>
        /// A set of allowed songs to stream
        /// </summary>
        public List<AudioClip> musicsToPlay;
    }
}