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
		[SerializeField] private TMP_Text         gameOverScore;
		[SerializeField] private AudioEvent       scoreTickSFX;
		[SerializeField] private KillFeedbackPool killFeedbackPool;
		[SerializeField] private ComboMeterUI     comboMeterUI;
		
		private AudioSource _audioSource;

		public int   killScore            = 10;
		public int   comboAdditionPerKill = 1;
		public float comboTimeLimit       = 25f;
		public int   maxCombo             = 3;

		private int   _displayedScore  = 0;
		private int   _currentScore    = 0;
		private int   _totalScore      = 0;
		private int   _comboMultiplier = 1;
		private bool  _comboIsActive;
		private float _comboTimeRemaining = 0;

		private void Awake()
		{
			Health.onAnyEnemyDeath += OnAnyEnemyDeathUpdate;
			_audioSource      =  GetComponent<AudioSource>();
			scoreText.font    =  inGameUI.inGameFont;
			score.font        =  inGameUI.inGameFont;
			score.text        =  _displayedScore.ToString();
		}

		private void OnDestroy()
		{
			Health.onAnyEnemyDeath -= OnAnyEnemyDeathUpdate;
		}

		/// <summary>
		///  Attempt to spend a amount of score.
		/// </summary>
		/// <param name="amount">The amount must be a positive number.</param>
		/// <returns>Returns if the spending was successful or not.</returns>
		public bool SpendScore(int amount)
		{
			if (_currentScore < amount) return false;
			if (amount    < 0) throw new ArgumentException("amount cannot negative");
			_currentScore -= amount;
			if (_displayedScore > _currentScore) _displayedScore = _currentScore;
			return true;
		}

		public void UpdateGameOverScoreText() => gameOverScore.text = _totalScore.ToString();

		private void OnAnyEnemyDeathUpdate(Vector3 position)
		{
			killFeedbackPool.SpawnKillTextFromPool(position);
			
			int delta = killScore * _comboMultiplier;
			_totalScore += delta;
			
			if (_displayedScore == _currentScore) {
				_currentScore += delta;
				StartCoroutine(UpdateScoreRoutine());
			} else {
				_currentScore += delta;
			}

			if (_comboMultiplier < maxCombo)
				_comboMultiplier += comboAdditionPerKill;

			if (_comboTimeRemaining > 0) {
				killFeedbackPool.SpawnComboTextFromPool(position);
				_comboTimeRemaining = comboTimeLimit;
			}
			else
				StartCoroutine(StartComboTimeRoutine());
		}

		private void UpdateScore(int newScore) => score.text = newScore.ToString();

		private IEnumerator UpdateScoreRoutine()
		{
			while (_displayedScore < _currentScore) {
				UpdateScore(++_displayedScore);
				//scoreTickSFX.PlayOneShot(_audioSource);
				// ServiceLocator.Audio.PlaySFX(scoreTickSFX);
				yield return new WaitForSeconds(0.002f + .75f / (_currentScore - _displayedScore)); 
			}
		}

		private IEnumerator StartComboTimeRoutine()
		{
			_comboTimeRemaining = comboTimeLimit;
			_comboIsActive      = true;
			while (_comboTimeRemaining > 0) {
				_comboTimeRemaining -= Time.deltaTime;
				comboMeterUI.UpdateUI(_comboTimeRemaining / comboTimeLimit);
				yield return null;
			}
			comboMeterUI.UpdateUI(0f);
			_comboMultiplier = 1;
			_comboIsActive   = false;
		}
	}
}