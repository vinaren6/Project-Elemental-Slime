using UnityEngine;

namespace _Project.Scripts.Audio
{
    public interface IAudio
    {
        public void PlaySFX();
        public void PlayBGM();
        public void PauseSFX();
        public void PauseBGM();
        public void StopSFX();
        public void StopBGM();
        public void StopAllSFX();
        public void StopAllBGM();
        public void StopAll();
    }
}