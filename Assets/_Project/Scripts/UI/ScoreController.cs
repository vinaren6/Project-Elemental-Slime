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
		
		private int   score           = 0;
		private int   comboMultiplier = 1;
		private bool  comboIsActive;
		private float comboTimeRemaining = 0;
		
		private void Start()
		{
			Health.onAnyDeath += OnDeathUpdate;
			scoreText.text    =  score.ToString();
		}
				
		private void OnDeathUpdate()
		{
			UpdateScore(killScore * comboMultiplier);
			
			comboMultiplier += comboAdditionPerKill;

			if (comboTimeRemaining > 0)
				comboTimeRemaining =  comboTimeLimit;
			else
				StartCoroutine(StartComboTimeRoutine());
		}

		private void UpdateScore(int scoreToAdd)
		{
			score += scoreToAdd;
			scoreText.text = score.ToString();
		}

		private IEnumerator StartComboTimeRoutine()
		{
			comboTimeRemaining = comboTimeLimit;
			comboIsActive      = true;
			while (comboTimeRemaining > 0)
			{
				comboTimeRemaining -= Time.deltaTime;
				print(comboTimeRemaining);
				yield return null;
			}

			comboMultiplier = 1;
			comboIsActive   = false;
		}
	}
}