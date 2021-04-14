using System;
using UnityEngine;
using _Project.Scripts.Managers;
using AudioType = _Project.Scripts.Audio.AudioType;

namespace _Project.Scripts.Events
{
    public class PlayAudio : MonoBehaviour
    {
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private AudioType audioType;

        public void Play()
        {
            switch (audioType)
            {
                case AudioType.SFX:
                    ServiceLocator.Audio.PlaySFX(audioClip);
                    break;
                case AudioType.BGM:
                    ServiceLocator.Audio.PlayBGM(audioClip);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}