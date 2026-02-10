using UnityEngine;

namespace SoundManager.VirtualListeners
{
    public class AudioListenerVirtual : MonoBehaviour
    {
        private void OnEnable() => VirtualAudioManager.Instance.RegisterListener(this);
        private void OnDisable() => VirtualAudioManager.Instance.UnregisterListener(this);
    }
}