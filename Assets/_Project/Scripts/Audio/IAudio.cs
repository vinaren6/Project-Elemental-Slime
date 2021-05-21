using UnityEngine;

namespace _Project.Scripts.Audio
{
    public interface IAudio
    {
        public void PlaySFX(AudioClip audioClip, float volume = 1f);
        public void PlaySFXDelay(AudioClip audioClip, float volume, float delay = 0f);
        public void PlayBGM(AudioClip audioClip, float volume = 1f);
        public void PauseSFX();
        public void PauseBGM();
        public void StopSFX();
        public void StopBGM();
        public void StopAllSFX();
        public void StopAllBGM();
        public void StopAll();
        public void UpdateVolume(AudioType audioType, float volume);
    }
}