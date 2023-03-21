using SoundManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundManager
{
    public class AudioRadioManager
    {
        public static Dictionary<int, AudioRadioPlaylistManager> playlistManagers = new Dictionary<int, AudioRadioPlaylistManager>();
        /// <summary>
        /// Request a playlist mananger for a specifiq player
        /// </summary>
        /// <param name="player">The player requesting</param>
        /// <returns>The playlist manager</returns>
        public static AudioRadioPlaylistManager ReqestPlaylistManager(AudioRadio player)
        {
            if (playlistManagers.ContainsKey(player.playlist.playlistID)) playlistManagers[player.playlist.playlistID].AddPlayer(player);
            else playlistManagers[player.playlist.playlistID] = new AudioRadioPlaylistManager(player.playlist, player);
            return playlistManagers[player.playlist.playlistID];
        }
        /// <summary>
        /// Remove a player from a playlist manager
        /// </summary>
        /// <param name="player">The player that want to remove from the list</param>
        public static void UnsubscribeFromPlaylistManager(AudioRadio player)
        {
            if (!playlistManagers.ContainsKey(player.playlist.playlistID)) return;
            playlistManagers[player.playlist.playlistID].RemovePlayer(player);
            if (!playlistManagers[player.playlist.playlistID].IsCurrentlyUsed) playlistManagers.Remove(player.playlist.playlistID);
        }
    }
}
