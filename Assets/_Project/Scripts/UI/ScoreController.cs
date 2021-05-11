using System;
using _Project.Scripts.HealthSystem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class ScoreController : MonoBehaviour
	{
		[SerializeField] private TMP_Text scoreText;

		private int score;

		private void Awake()
		{
			Health.onAnyDeath += UpdateScore;
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
	}
}