using System;
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

        private void Start() => volume.value = PlayerPrefs.GetFloat("audio" + audioType, audioType == AudioType.BGM ? 0.15f : 1f);

        public void UpdateVolume()
        {
            if (mute.isOn) return;
            ServiceLocator.Audio.UpdateVolume(audioType, volume.value);
        }

        public void UpdateMute()
        {
            float newVolume = mute.isOn ? 0 : volume.value;
            PlayerPrefs.SetFloat("audio" + audioType, newVolume);
            ServiceLocator.Audio.UpdateVolume(audioType, newVolume);
        }
    }
}