using SoundManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SoundManager
{
    public class AudioRadioPlaylistManager
    {
        public static float GetCurrentLiveTime()
        {
            return Time.unscaledTime;
        }

        AudioRadioPlaylist playlist;
        List<AudioRadio> players;
        float lastStartTime;
        AudioClip currentClip;

        /// <summary>
        /// Is at least one player using this playlist manager at the moment ?
        /// </summary>
        public bool IsCurrentlyUsed { get { return players.Count > 0; } }

        public AudioRadioPlaylistManager(AudioRadioPlaylist playlist, AudioRadio initialPlayer)
        {
            this.playlist = playlist;
            players = new List<AudioRadio> { initialPlayer };
            PickNewSong();
        }

        public void AddPlayer(AudioRadio player)
        {
            players.Add(player);
        }
        public void RemovePlayer(AudioRadio player)
        {
            players.Remove(player);
        }
        public AudioRadioLiveClip GetLiveInfos()
        {
            float durationFromStart = GetCurrentLiveTime() - lastStartTime;
            if (durationFromStart > currentClip.length)
            {
                PickNewSong();
                return new AudioRadioLiveClip(currentClip, 0);
            }
            else
            {
                return new AudioRadioLiveClip(currentClip, durationFromStart);
            }
        }

        private void PickNewSong()
        {
            lastStartTime = GetCurrentLiveTime();
            currentClip = playlist.musicsToPlay[Random.Range(0, playlist.musicsToPlay.Count)];
        }
    }
}
