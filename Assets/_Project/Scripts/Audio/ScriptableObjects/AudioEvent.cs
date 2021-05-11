using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Audio.ScriptableObjects
{
	[CreateAssetMenu]
	public class AudioEvent : ScriptableObject
	{
		public AudioClip[] clips;

		public RangedFloat volume;

		[MinMaxRange(0, 2)] public RangedFloat pitch;

		public void Play(AudioSource audioSource)
		{
			if (audioSource.isPlaying) return;
			if (clips.Length == 0) return;

			audioSource.clip   = clips[Random.Range(0, clips.Length)];
			audioSource.volume = Random.Range(volume.minValue, volume.maxValue);
			audioSource.pitch  = Random.Range(pitch.minValue,  pitch.maxValue);
			audioSource.Play();
		}

		public void PlayOneShot(AudioSource audioSource)
		{
			if (clips.Length == 0) return;

			audioSource.volume = Random.Range(volume.minValue, volume.maxValue);
			audioSource.pitch  = Random.Range(pitch.minValue,  pitch.maxValue);
			AudioClip clip = clips[Random.Range(0, clips.Length)];
			audioSource.PlayOneShot(clip);
		}
	}
}