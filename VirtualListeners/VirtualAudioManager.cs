#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace SoundManager.VirtualListeners
{
    public class VirtualAudioManager : MonoBehaviour
    {
        /// <summary>
        /// Lazy singleton accessor. Creates the instance if it doesn't exist, and ensures it persists across scene loads.
        /// Create the VirtualAudioManager and AudioListener on demand. It will not create this if not used.
        /// </summary>
        public static VirtualAudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("VirtualAudioManager");
                    _instance = obj.AddComponent<VirtualAudioManager>();
                    _instance.SetupRealAudioListener();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }

        private static VirtualAudioManager? _instance;
        private List<AudioListenerVirtual> _listeners = new List<AudioListenerVirtual>();
        private AudioListener _realAudioListener = null!;

        public void RegisterListener(AudioListenerVirtual listener) => _listeners.Add(listener);
        public void UnregisterListener(AudioListenerVirtual listener) => _listeners.Remove(listener);

        public AudioListenerVirtual? GetClosestListener(Vector3 sourcePos)
        {
            AudioListenerVirtual? closest = null;
            float minDst = float.MaxValue;

            for (int i = 0; i < _listeners.Count; i++)
            {
                if (_listeners[i] == null) continue;
                float dst = Vector3.SqrMagnitude(_listeners[i].transform.position - sourcePos);
                if (dst < minDst)
                {
                    minDst = dst;
                    closest = _listeners[i];
                }
            }
            return closest;
        }

        private void SetupRealAudioListener()
        {
            _realAudioListener = gameObject.AddComponent<AudioListener>();
        }
    }
}