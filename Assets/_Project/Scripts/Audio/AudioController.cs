using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Audio
{
	public class AudioController : IAudio
	{
		private readonly AudioSource _audioSourceBGM;
		private readonly AudioSource _audioSourceSFX;
		private          float       _SFXVolume = 1f;
		private          float       _BGMVolume = 1f;
		private          float       _masterVolume = 1f;
		private          bool        _muteBGM;
		private          bool        _muteMaster;
		private          bool        _muteSfx;
		
		public AudioController()
		{
			var audioPlayerObj = new GameObject();
			audioPlayerObj.transform.name = "Audio Sources";
			Object.DontDestroyOnLoad(audioPlayerObj);
			_audioSourceSFX      = audioPlayerObj.AddComponent<AudioSource>();
			_audioSourceBGM      = audioPlayerObj.AddComponent<AudioSource>();
			_audioSourceBGM.loop = true;

			//loadVolume
			_audioSourceSFX.volume = _SFXVolume = PlayerPrefs.GetFloat("audioSFX", 1f);
			_audioSourceBGM.volume = _BGMVolume = PlayerPrefs.GetFloat("audioBGM", 0.15f);
			UpdateVolume(AudioType.Master, PlayerPrefs.GetFloat("audioMaster", 1f));

			//load mute
			_muteSfx    = PlayerPrefs.GetInt("audioMuteSFX",    0) == 1;
			_muteBGM    = PlayerPrefs.GetInt("audioMuteBGM",    0) == 1;
			_muteMaster = PlayerPrefs.GetInt("audioMuteMaster", 0) == 1;
		}

		public void Mute(AudioType audioType, bool mute)
		{
			switch (audioType) {
				case AudioType.SFX:
					_muteSfx = mute;
					break;
				case AudioType.BGM:
					_muteBGM = mute;
					break;
				case AudioType.Master:
					_muteMaster = mute;
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
			}
		}

		public void PlaySFX(AudioClip audioClip, float volume = 1f) => _audioSourceSFX.PlayOneShot(audioClip, volume);

		public void PlaySFXDelay(AudioClip audioClip, float volume, float delay = 0f)
		{
			_audioSourceSFX.clip   = audioClip;
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
			_audioSourceBGM.clip = audioClip;
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
					volume = _muteMaster ? 0f : volume;
					float masterVolumeDelta = volume / _masterVolume;
					_masterVolume          = volume;
					_audioSourceBGM.volume = _BGMVolume * masterVolumeDelta;
					_audioSourceSFX.volume = _SFXVolume * masterVolumeDelta;
					return;
				case AudioType.BGM:
					_BGMVolume             = volume;
					_audioSourceBGM.volume = _muteBGM ? 0f : volume * _masterVolume;
					return;
				case AudioType.SFX:
					_SFXVolume             = volume;
					_audioSourceSFX.volume = _muteSfx ? 0f : volume * _masterVolume;
					return;
				default:
					throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
			}
		}
	}
}