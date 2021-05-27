using System;
using UnityEngine;
using UnityEngine.Audio;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Audio
{
	public class AudioController : IAudio
	{
		private readonly AudioSource _audioSourceBGM;
		private readonly AudioSource _audioSourceSFX;
		private readonly AudioSource _audioSourceMaster;
		private          float       _BGMVolume    = 1f;
		private          float       _masterVolume = 1f;
		private readonly AudioMixer  _mixer;
		private          bool        _muteBGM;
		private          bool        _muteMaster;
		private          bool        _muteSfx;
		private          float       _SFXVolume = 1f;

		public AudioController()
		{
			//initialize
			_mixer = Resources.Load<AudioMixer>("AudioMixer");

			var audioPlayerObj = new GameObject();
			audioPlayerObj.transform.name = "Audio Sources";
			Object.DontDestroyOnLoad(audioPlayerObj);
			_audioSourceSFX      = audioPlayerObj.AddComponent<AudioSource>();
			_audioSourceBGM      = audioPlayerObj.AddComponent<AudioSource>();
			_audioSourceMaster   = audioPlayerObj.AddComponent<AudioSource>();
			_audioSourceBGM.loop = true;

			_audioSourceSFX.outputAudioMixerGroup    = _mixer.FindMatchingGroups("SFX")[0];
			_audioSourceBGM.outputAudioMixerGroup    = _mixer.FindMatchingGroups("BGM")[0];
			_audioSourceMaster.outputAudioMixerGroup = _mixer.FindMatchingGroups("Master")[0];

			//loadVolume
			_SFXVolume    = PlayerPrefs.GetFloat("audioSFX",    1f);
			_BGMVolume    = PlayerPrefs.GetFloat("audioBGM",    0.15f);
			_masterVolume = PlayerPrefs.GetFloat("audioMaster", 1f);

			//load mute
			_muteSfx    = PlayerPrefs.GetInt("audioMuteSFX",    0) == 1;
			_muteBGM    = PlayerPrefs.GetInt("audioMuteBGM",    0) == 1;
			_muteMaster = PlayerPrefs.GetInt("audioMuteMaster", 0) == 1;

			//apply audio
			UpdateVolume(AudioType.SFX,    _SFXVolume);
			UpdateVolume(AudioType.BGM,    _BGMVolume);
			UpdateVolume(AudioType.Master, _masterVolume);
		}

		public void Mute(AudioType audioType, bool mute)
		{
			switch (audioType) {
				case AudioType.SFX:
					_muteSfx = mute;
					_mixer.SetFloat("SFX", _muteSfx ? -80f : VolumeToDB(_SFXVolume));
					break;
				case AudioType.BGM:
					_muteBGM = mute;
					_mixer.SetFloat("BGM", _muteBGM ? -80f : VolumeToDB(_BGMVolume));
					break;
				case AudioType.Master:
					_muteMaster = mute;
					_mixer.SetFloat("Master", _muteMaster ? -80f : VolumeToDB(_masterVolume));
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
			}
		}

		public void PlaySFX(AudioClip audioClip, float volume = 1f) => _audioSourceSFX.PlayOneShot(audioClip, volume);
		public void PlayMaster(AudioClip audioClip, float volume = 1f) => _audioSourceMaster.PlayOneShot(audioClip, volume);
		public void PlaySFXDelay(AudioClip audioClip, float volume, float delay = 0f)
		{
			_audioSourceSFX.clip = audioClip;
			//_audioSourceSFX.volume = volume;
			_audioSourceSFX.PlayDelayed(delay);
		}

		public void PlayBGM(AudioClip audioClip, float volume = 1f)
		{
			if (_audioSourceBGM.isPlaying) {
				if (audioClip == _audioSourceBGM.clip) return;
				_audioSourceBGM.PlayOneShot(audioClip, volume);
				// _audioSourceBGM.PlayOneShot(audioClip);
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
			_audioSourceMaster.Stop();
		}

		public void UpdateVolume(AudioType audioType, float volume)
		{
			switch (audioType) {
				case AudioType.Master:
					_mixer.SetFloat("Master", _muteMaster ? -80f : VolumeToDB(volume));
					//volume = _muteMaster ? 0f : volume;
					//float masterVolumeDelta = volume / _masterVolume;
					_masterVolume = volume;
					//_audioSourceBGM.volume = _BGMVolume * masterVolumeDelta;
					//_audioSourceSFX.volume = _SFXVolume * masterVolumeDelta;
					return;
				case AudioType.BGM:
					_mixer.SetFloat("BGM", _muteBGM ? -80f : VolumeToDB(volume));
					_BGMVolume = volume;
					//_audioSourceBGM.volume = _muteBGM ? 0f : volume * _masterVolume;
					return;
				case AudioType.SFX:
					_mixer.SetFloat("SFX", _muteSfx ? -80f : VolumeToDB(volume));
					_SFXVolume = volume;
					//_audioSourceSFX.volume = _muteSfx ? 0f : volume * _masterVolume;
					return;
				default:
					throw new ArgumentOutOfRangeException(nameof(audioType), audioType, null);
			}
		}

		private float VolumeToDB(float t)
		{
			float sliderFeel = t * 2 - 1;
			sliderFeel *= sliderFeel;
			sliderFeel =  1 - sliderFeel;
			
			return Mathf.Lerp(-80, 0, t + sliderFeel * 0.3f);
		}
	}
}