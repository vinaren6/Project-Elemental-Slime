using System.Collections;
using _Project.Scripts.HealthSystem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class ScoreController : MonoBehaviour
	{
		[SerializeField] private TMP_Text scoreText;

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
			Health.onAnyDeath += OnDeathUpdate;
			scoreText.text    =  currentScore.ToString();
		}
				
		private void OnDeathUpdate()
		{
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
				// Play sound?
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