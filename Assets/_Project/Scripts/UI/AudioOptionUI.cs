using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.UI;
using AudioType = _Project.Scripts.Audio.AudioType;

namespace _Project.Scripts.UI
{
    public class AudioOptionUI : MonoBehaviour
    {
        [SerializeField] private Slider volume;
        [SerializeField] private Toggle mute;
        [SerializeField] private AudioType audioType;

        public void UpdateVolume()
        {
            if (mute.isOn)
            {
                return;
            }
            ServiceLocator.Audio.UpdateVolume(audioType, volume.value);
        }

        public void UpdateMute()
        {
            ServiceLocator.Audio.UpdateVolume(audioType, mute.isOn ? 0 : volume.value);
        }
    }
}