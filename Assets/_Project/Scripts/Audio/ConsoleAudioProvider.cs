using UnityEngine;

namespace _Project.Scripts.Audio
{
    public class ConsoleAudioProvider : IAudio
    {
        #region Methods

        #region IAudio Interface

        public void PlaySFX(AudioClip audioClip, float volume = 1f)
        {
            Debug.Log($"{nameof(PlaySFX)} - {audioClip.name}");
        }

        public void PlaySFX(AudioClip audioClip, float volume = 1, float delay = 0)
        {
            throw new System.NotImplementedException();
        }

        public void PlayBGM(AudioClip audioClip, float volume = 1f)
        {
            Debug.Log($"{nameof(PlayBGM)}");
        }

        public void PauseSFX()
        {
            Debug.Log($"{nameof(PauseSFX)}");
        }

        public void PauseBGM()
        {
            Debug.Log($"{nameof(PauseBGM)}");
        }

        public void StopSFX()
        {
            Debug.Log($"{nameof(StopSFX)}");
        }

        public void StopBGM()
        {
            Debug.Log($"{nameof(StopBGM)}");
        }

        public void StopAllSFX()
        {
            Debug.Log($"{nameof(StopAllSFX)}");
        }

        public void StopAllBGM()
        {
            Debug.Log($"{nameof(StopAllBGM)}");
        }

        public void StopAll()
        {
            Debug.Log($"{nameof(StopAll)}");
        }

        public void UpdateVolume(AudioType audioType, float volume)
        {
            Debug.Log($"{nameof(UpdateVolume)} - audioType: {audioType} - volume: {volume}");
        }

        #endregion

        #endregion
    }
}