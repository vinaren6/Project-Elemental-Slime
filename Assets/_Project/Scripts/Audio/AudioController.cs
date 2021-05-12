using UnityEngine;

namespace _Project.Scripts.Audio
{
	public class AudioController : IAudio
	{
		private GameObject _audioPlayerObj;

		private AudioSource _audioSourceSFX, _audioSourceBGM;


		public AudioController()
		{
			_audioPlayerObj      ??= new GameObject();
			_audioSourceSFX      ??= _audioPlayerObj.AddComponent<AudioSource>();
			_audioSourceBGM      ??= _audioPlayerObj.AddComponent<AudioSource>();
			_audioSourceBGM.loop =   true;
		}

		public void PlaySFX(AudioClip audioClip, float volume = 1f) => _audioSourceSFX.PlayOneShot(audioClip, volume);

		public void PlayBGM(AudioClip audioClip, float volume = 1f)
		{
			if (_audioSourceBGM.isPlaying) {
				_audioSourceBGM.PlayOneShot(audioClip, volume);
				return;
			}
			_audioSourceBGM.clip   = audioClip;
			_audioSourceBGM.volume = volume;
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

		public void UpdateVolume(AudioType audioType, float volume) { }
		
	}
}