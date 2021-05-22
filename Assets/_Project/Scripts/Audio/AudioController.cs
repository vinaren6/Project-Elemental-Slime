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

			//loadVolume
			_audioSourceSFX.volume = PlayerPrefs.GetFloat("audioSFX", 1f);
			_audioSourceBGM.volume = PlayerPrefs.GetFloat("audioBGM", 0.15f);
			UpdateVolume(AudioType.Master, PlayerPrefs.GetFloat("audioMaster", 1f));
		}

		public void PlaySFX(AudioClip audioClip, float volume = 1f) => _audioSourceSFX.PlayOneShot(audioClip, volume);

		public void PlaySFXDelay(AudioClip audioClip, float volume, float delay = 0f)
		{
			_audioSourceSFX.clip = audioClip;
			_audioSourceSFX.volume = volume;
			_audioSourceSFX.PlayDelayed(delay);
		}
		
		public void PlayBGM(AudioClip audioClip, float volume = 1f)
		{
			if (_audioSourceBGM.isPlaying) {
				return;
				//_audioSourceBGM.PlayOneShot(audioClip, volume);
				_audioSourceBGM.PlayOneShot(audioClip);
				return;
			}
			
			//_audioSourceBGM.volume = volume * _masterVolume;
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