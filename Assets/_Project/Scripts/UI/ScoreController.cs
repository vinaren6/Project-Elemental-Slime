using System.Collections;
using _Project.Scripts.Audio.ScriptableObjects;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class ScoreController : MonoBehaviour
	{
		[SerializeField] private TMP_Text   scoreText;
		[SerializeField] private AudioEvent scoreTickSFX;
		[SerializeField] private KillFeedbackPool killFeedbackPool;

		private AudioSource _audioSource;

		public  int   killScore            = 10;
		public  int   comboAdditionPerKill = 1;
		public  float comboTimeLimit       = 25f;
		
		private int   currentScore    = 0;
		private int   newScore    = 0;
		private int   comboMultiplier = 1;
		private bool  comboIsActive;
		private float comboTimeRemaining = 0;
		
		private void Start()
		{
			Health.onAnyDeath += OnAnyDeathUpdate;
			scoreText.text    =  currentScore.ToString();
			_audioSource      =  GetComponent<AudioSource>();
		}
				
		private void OnAnyDeathUpdate(Vector3 position)
		{
			killFeedbackPool.SpawnFromPool(position);
			
			if (currentScore == newScore) {
				newScore = currentScore + killScore * comboMultiplier;
				StartCoroutine(UpdateScoreRoutine());
			} 
			else {
				newScore = currentScore + killScore * comboMultiplier;	
			}

			comboMultiplier += comboAdditionPerKill;

			if (comboTimeRemaining > 0)
				comboTimeRemaining =  comboTimeLimit;
			else
				StartCoroutine(StartComboTimeRoutine());
		}

		private void UpdateScore()
		{
			currentScore++;
			scoreText.text = currentScore.ToString();
		}

		private IEnumerator UpdateScoreRoutine()
		{
			while (currentScore < newScore) {
				UpdateScore();
				scoreTickSFX.PlayOneShot(_audioSource);
				// ServiceLocator.Audio.PlaySFX(scoreTickSFX);
				yield return new WaitForSeconds(0.05f);
			}
		}

		private IEnumerator StartComboTimeRoutine()
		{
			comboTimeRemaining = comboTimeLimit;
			comboIsActive      = true;
			while (comboTimeRemaining > 0)
			{
				comboTimeRemaining -= Time.deltaTime;
				// print(comboTimeRemaining);
				yield return null;
			}

			comboMultiplier = 1;
			comboIsActive   = false;
		}
	}
}