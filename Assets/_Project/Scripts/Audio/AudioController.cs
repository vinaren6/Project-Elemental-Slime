using System;
using UnityEngine;

namespace _Project.Scripts.Audio
{
	public class AudioController : IAudio
	{
		private readonly AudioSource _audioSourceSFX;
		private readonly AudioSource _audioSourceBGM;
		private          float       _masterVolume = 1f;

		public AudioController()
		{
			var audioPlayerObj = new GameObject();
			audioPlayerObj.transform.name = "Audio Sources";
			UnityEngine.Object.DontDestroyOnLoad(audioPlayerObj);
			_audioSourceSFX      = audioPlayerObj.AddComponent<AudioSource>();
			_audioSourceBGM      = audioPlayerObj.AddComponent<AudioSource>();
			_audioSourceBGM.loop = true;
		}

		public void PlaySFX(AudioClip audioClip, float volume = 1f) => _audioSourceSFX.PlayOneShot(audioClip, volume);

		public void PlayBGM(AudioClip audioClip, float volume = 1f)
		{
			if (_audioSourceBGM.isPlaying) {
				_audioSourceBGM.PlayOneShot(audioClip, volume);
				return;
			}

			if (volume != 0)
				_audioSourceBGM.volume = volume * _masterVolume;
			_audioSourceBGM.clip   = audioClip;
			_audioSourceBGM.Play();
		}

		public void PauseSFX() => _audioSourceSFX.Pause();

		public void PauseBGM() => _audioSourceBGM.Pause();

		public void StopSFX() => _audioSourceSFX.Stop();

		public void StopBGM() => _audioSourceBGM.Stop();

		public void StopAllSFX() => StopSFX();

		public void StopAllBGM() => StopBGM();

		public void StopAll()
		{
			StopAllSFX();
			StopAllBGM();
		}

		public void UpdateVolume(AudioType audioType, float volume)
		{
			switch (audioType) {
				case AudioType.Master:
					float masterVolumeDelta = volume / _masterVolume;
					_masterVolume          =  volume;
					_audioSourceBGM.volume *= masterVolumeDelta;
					_audioSourceSFX.volume *= masterVolumeDelta;
					return;
				case AudioType.BGM:
					_audioSourceBGM.volume = volume * _masterVolume;
					return;
				case AudioType.SFX:
					_audioSourceSFX.volume = volume * _masterVolume;
					return;
				default:
					throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
			}
		}
	}
}