using _Project.Scripts.HealthSystem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class ScoreController : MonoBehaviour
	{
		[SerializeField] private TMP_Text scoreText;

		private int score = 0;

		private void Awake()
		{
			Health.onAnyDeath += UpdateScore;
		}

		private void UpdateScore()
		{
			score++;
			scoreText.text = score.ToString();
		}
	}
}