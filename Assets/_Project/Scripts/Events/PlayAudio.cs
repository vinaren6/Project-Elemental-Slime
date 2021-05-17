using System;
using _Project.Scripts.Audio.ScriptableObjects;
using _Project.Scripts.Managers;
using UnityEngine;
using AudioType = _Project.Scripts.Audio.AudioType;

namespace _Project.Scripts.Events
{
	public class PlayAudio : MonoBehaviour
	{
		[SerializeField] private AudioClip audioClip;
		[SerializeField] private AudioType audioType;

		[Space] [MinMaxRange(0.001f, 1f)] [SerializeField]
		private float volume = 1;

		[SerializeField] private bool playOnStart;

		private void Start()
		{
			if (playOnStart) Play();
		}

		public void Play()
		{
			switch (audioType) {
				case AudioType.SFX:
					ServiceLocator.Audio.PlaySFX(audioClip, volume);
					break;
				case AudioType.BGM:
					ServiceLocator.Audio.PlayBGM(audioClip, volume);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}