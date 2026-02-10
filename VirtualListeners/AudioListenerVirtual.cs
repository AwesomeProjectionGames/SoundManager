using UnityEngine;

namespace SoundManager.VirtualListeners
{
    /// <summary>
    /// Represents a virtual listener in the scene. 
    /// Should be attached to objects that represent the listener's position (e.g. cameras in split-screen).
    /// Registers itself with the VirtualAudioManager upon enabling.
    /// </summary>
    public class AudioListenerVirtual : MonoBehaviour
    {
        private void OnEnable() => VirtualAudioManager.Instance.RegisterListener(this);
        private void OnDisable() => VirtualAudioManager.Instance.UnregisterListener(this);
    }
}