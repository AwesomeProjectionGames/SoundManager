using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace SoundManager
{
    /// <summary>
    /// A audio source with a playlist synced with other audiosource who have the same playlist id
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AudioRadio : MonoBehaviour
    {
        public AudioRadioPlaylist playlist;

        [SerializeField]
        private bool playByDefault = true;

        private AudioRadioPlaylistManager currentPlaylistManager;
        private bool isPlaying = false;
        private AudioSource radio;

        private void Awake()
        {
            radio = GetComponent<AudioSource>();
        }
        private void OnEnable()
        {
            if(playByDefault) Play();
        }
        private void OnDisable()
        {
            Stop();
        }
        private void Update()
        {
            if (!isPlaying) return;
            if (!radio.isPlaying) UpdateSong();
        }
        void UpdateSong()
        {
            AudioRadioLiveClip liveInfos = currentPlaylistManager.GetLiveInfos();
            radio.clip = liveInfos.currentClipPlayed;
            radio.Play();
            radio.time = liveInfos.currentTimeOfTheClip;
        }
        public void Play()
        {
            if (isPlaying) return;
            isPlaying = true;
            currentPlaylistManager = AudioRadioManager.ReqestPlaylistManager(this);
            UpdateSong();
        }
        public void Stop()
        {
            if (!isPlaying) return;
            radio.Stop();
            isPlaying = false;
            AudioRadioManager.UnsubscribeFromPlaylistManager(this);
        }
    }
}