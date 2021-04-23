using UnityEngine;

namespace _Project.Scripts.Audio
{
    public class AudioController : IAudio
    {
        private readonly AudioSource[] _audioSources;

        public AudioController()
        {
            int maxAudioSources = 16;
            float volume = 0.05f;
            _audioSources = new AudioSource[maxAudioSources];
            
            GameObject audioGO = new GameObject();

            for (int i = 0; i < maxAudioSources; i++)
            {
                _audioSources[i] = audioGO.AddComponent<AudioSource>();
                _audioSources[i].volume = volume;
            }
        }
        
        public void PlaySFX(AudioClip audioClip)
        {
            if (audioClip == null) return;
            AudioSource audioSource = GetAvailableAudioSource();
            if (audioSource == null) return;
            audioSource.clip = audioClip;
            audioSource.Play();
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

        private AudioSource GetAvailableAudioSource()
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                if (!audioSource.isPlaying)
                    return audioSource;
            }

            return null;
        }
    }
}