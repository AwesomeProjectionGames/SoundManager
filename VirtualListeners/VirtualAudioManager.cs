#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace SoundManager.VirtualListeners
{
    /// <summary>
    /// Manages virtual audio listeners and handles the setup of the real audio listener in the scene.
    /// Acts as a central registry for all virtual listeners to help audio sources determine where they should be spatialized relative to.
    /// </summary>
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

        // Pool for proxy audio sources to avoid constant instantiation/destruction
        private Queue<AudioSource> _proxyPool = new Queue<AudioSource>();

        /// <summary>
        /// Retrieves an AudioSource from the pool or creates a new one.
        /// </summary>
        public AudioSource GetProxySource()
        {
            AudioSource source;
            if (_proxyPool.Count > 0)
            {
                source = _proxyPool.Dequeue();
            }
            else
            {
                GameObject obj = new GameObject("AudioProxy");
                obj.transform.SetParent(transform);
                source = obj.AddComponent<AudioSource>();
                source.playOnAwake = false;
            }
            
            source.gameObject.SetActive(true);
            return source;
        }

        /// <summary>
        /// Returns an AudioSource to the pool.
        /// </summary>
        public void ReturnProxySource(AudioSource source)
        {
            if (source == null) return;
            
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
            _proxyPool.Enqueue(source);
        }

        /// <summary>
        /// Registers a virtual listener with the manager.
        /// </summary>
        /// <param name="listener">The virtual listener to register.</param>
        public void RegisterListener(AudioListenerVirtual listener) => _listeners.Add(listener);

        /// <summary>
        /// Unregisters a virtual listener from the manager.
        /// </summary>
        /// <param name="listener">The virtual listener to unregister.</param>
        public void UnregisterListener(AudioListenerVirtual listener) => _listeners.Remove(listener);

        /// <summary>
        /// Finds the virtual listener closest to a given position.
        /// </summary>
        /// <param name="sourcePos">The position to check against.</param>
        /// <returns>The closest AudioListenerVirtual, or null if none are registered.</returns>
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