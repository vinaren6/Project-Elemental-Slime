using System.Collections;
using _Project.Scripts.HealthSystem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class ScoreController : MonoBehaviour
	{
		[SerializeField] private TMP_Text scoreText;

		public int   killScore      = 10;
		public float comboTimeLimit = 25f;
		public bool  comboIsActive;
		
		private int score;

		private float comboTimeRemaining = 0;

		private void Awake()
		{
			Health.onAnyDeath += OnDeathUpdateScore;
		}

		private void Start()
		{
			score          = 0;
			scoreText.text = score.ToString();
		}

		private void UpdateScore()
		{
			score++;
			scoreText.text = score.ToString();
		}
		
		private void OnDeathUpdateScore()
		{
			score += killScore;
			scoreText.text = score.ToString();

			if (comboTimeRemaining > 0) 
				comboTimeRemaining = comboTimeLimit;
			else
				StartCoroutine(StartComboTimeRoutine());
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
			comboIsActive = false;
		}
	}
}