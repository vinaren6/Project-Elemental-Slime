using UnityEngine;

namespace _Project.Scripts.Audio
{
    public class ConsoleAudioProvider : IAudio
    {
        #region Methods

        #region IAudio Interface

        public void PlaySFX()
        {
            Debug.Log($"{nameof(PlaySFX)}");
        }

        public void PlayBGM()
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

        #endregion

        #endregion
    }
}