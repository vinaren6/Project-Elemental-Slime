using UnityEngine;

namespace _Project.Scripts.Audio
{
    public class NullAudioProvider : IAudio
    {
        #region Methods

        #region IAudio Interface

        public void Mute(AudioType audioType, bool mute) => throw new System.NotImplementedException();

        public void PlaySFX(AudioClip audioClip, float volume = 1f)
        {
        }

        public void PlayMaster(AudioClip audioClip, float volume = 1) => throw new System.NotImplementedException();

        public void PlaySFXDelay(AudioClip audioClip, float volume, float delay = 0)
        {
        }

        public void PlayBGM(AudioClip audioClip, float volume = 1f)
        {
        }

        public void PauseSFX()
        {
        }

        public void PauseBGM()
        {
        }

        public void StopSFX()
        {
        }

        public void StopBGM()
        {
        }

        public void StopAllSFX()
        {
        }

        public void StopAllBGM()
        {
        }

        public void StopAll()
        {
        }

        public void UpdateVolume(AudioType audioType, float volume)
        {
        }

        #endregion

        #endregion
    }
}