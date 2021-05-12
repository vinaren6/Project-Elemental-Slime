using System;
using System.Collections;
using System.Linq.Expressions;
using _Project.Scripts.Audio.ScriptableObjects;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
using _Project.Scripts.UI.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class ScoreController : MonoBehaviour
	{
		[SerializeField] private InGameUI         inGameUI;
		[SerializeField] private TMP_Text         scoreText;
		[SerializeField] private TMP_Text         score;
		[SerializeField] private AudioEvent       scoreTickSFX;
		[SerializeField] private KillFeedbackPool killFeedbackPool;

		private AudioSource _audioSource;

		public int   killScore            = 10;
		public int   comboAdditionPerKill = 1;
		public float comboTimeLimit       = 25f;

		private int   _currentScore    = 0; //the displayed score
		private int   _newScore        = 0; //the actual score
		private int   _totalScore      = 0; //tracks total score. 
		private int   _comboMultiplier = 1;
		private bool  _comboIsActive;
		private float _comboTimeRemaining = 0;

		private void Awake()
		{
			Health.onAnyDeath += OnAnyDeathUpdate;
			_audioSource      =  GetComponent<AudioSource>();
			scoreText.font    =  inGameUI.inGameFont;
			score.font        =  inGameUI.inGameFont;
			score.text        =  _currentScore.ToString();
		}

		/// <summary>
		///  Attempt to spend a amount of score.
		/// </summary>
		/// <param name="amount">The amount must be a positive number.</param>
		/// <returns>Returns if the spending was successful or not.</returns>
		public bool SpendScore(int amount)
		{
			if (_newScore < amount) return false;
			if (amount    < 0) throw new ArgumentException("amount cannot negative");
			_newScore -= amount;
			return true;
		}

		private void OnAnyDeathUpdate(Vector3 position)
		{
			killFeedbackPool.SpawnKillTextFromPool(position);
			killFeedbackPool.SpawnComboTextFromPool(position);

			int delta = killScore * _comboMultiplier;
			_totalScore += delta;
			if (_currentScore == _newScore) {
				_newScore += delta;
				StartCoroutine(UpdateScoreRoutine());
			} else { _newScore += delta; }

			_comboMultiplier += comboAdditionPerKill;

			if (_comboTimeRemaining > 0)
				_comboTimeRemaining = comboTimeLimit;
			else
				StartCoroutine(StartComboTimeRoutine());
		}

		private void UpdateScore()
		{
			_currentScore++;
			score.text = _currentScore.ToString();
		}

		private IEnumerator UpdateScoreRoutine()
		{
			while (_currentScore < _newScore) {
				UpdateScore();
				scoreTickSFX.PlayOneShot(_audioSource);
				// ServiceLocator.Audio.PlaySFX(scoreTickSFX);
				yield return new WaitForSeconds(0.05f);
			}
		}

		private IEnumerator StartComboTimeRoutine()
		{
			_comboTimeRemaining = comboTimeLimit;
			_comboIsActive      = true;
			while (_comboTimeRemaining > 0) {
				_comboTimeRemaining -= Time.deltaTime;
				// print(comboTimeRemaining);
				yield return null;
			}

			_comboMultiplier = 1;
			_comboIsActive   = false;
		}
	}
}