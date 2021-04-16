using UnityEngine;

namespace _Project.Scripts.Audio
{
    public class NullAudioProvider : IAudio
    {
        #region Methods

        #region IAudio Interface

        public void PlaySFX(AudioClip audioClip)
        {
        }

        public void PlayBGM(AudioClip audioClip)
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