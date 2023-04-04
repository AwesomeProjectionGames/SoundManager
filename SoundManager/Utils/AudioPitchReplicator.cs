using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SoundManager
{
    public class AudioPitchReplicator : MonoBehaviour
    {
        [SerializeField] AudioSource source;
        [SerializeField] AudioSource target;
        [SerializeField] float multiplier = 1f;
        [SerializeField] float offset = 0f;

        private void Update()
        {
            target.pitch = source.pitch * multiplier + offset;
        }
    }
}