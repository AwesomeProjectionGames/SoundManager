using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SoundManager
{
    public struct AudioRadioLiveClip
    {
        public AudioRadioLiveClip(AudioClip currentClipPlayed, float currentTimeOfTheClip)
        {
            this.currentClipPlayed = currentClipPlayed;
            this.currentTimeOfTheClip = currentTimeOfTheClip;
        }
        public AudioClip currentClipPlayed;
        public float currentTimeOfTheClip;
    }
}
