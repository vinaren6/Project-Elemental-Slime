using System;
using System.Collections;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
using _Project.Scripts.UI.ScriptableObjects;
using _Project.Scripts.WaveSystem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Score
{
	public class ScoreController : MonoBehaviour
	{
		private static ScoreController _instance;

		[SerializeField] private InGameUI         inGameUI;
		[SerializeField] private TMP_Text         scoreText;
		[SerializeField] private TMP_Text         score;
		[SerializeField] private TMP_Text         gameOverScore;
		[SerializeField] private AudioClip        scoreTickSFX;
		[SerializeField] private KillFeedbackPool killFeedbackPool;
		[SerializeField] private ComboMeterUI     comboMeterUI;
		[SerializeField] private TMP_Text         waveText;

		public int   killScore            = 10;
		public int   pickupScore          = 5;
		public int   comboAdditionPerKill = 1;
		public float comboTimeLimit       = 25f;
		public int   maxCombo             = 3;

		private AudioSource _audioSource;
		private bool        _comboIsActive;
		private int         _comboMultiplier = 1;
		private float       _comboTimeRemaining;
		private int         _currentScore;

		private int _displayedScore;

		private int _kills;
		private int _pickups;
		private int _totalScore;

		private void Awake()
		{
			Health.onAnyEnemyDeath += OnAnyEnemyDeathUpdate;
			_instance              =  this;
			_audioSource           =  GetComponent<AudioSource>();
			scoreText.font         =  inGameUI.inGameFont;
			score.font             =  inGameUI.inGameFont;
			score.text             =  _displayedScore.ToString();
		}

		private void OnDestroy() => Health.onAnyEnemyDeath -= OnAnyEnemyDeathUpdate;

		/// <summary>
		///     Attempt to spend a amount of score.
		/// </summary>
		/// <param name="amount">The amount must be a positive number.</param>
		/// <returns>Returns if the spending was successful or not.</returns>
		public bool SpendScore(int amount)
		{
			if (_currentScore < amount) return false;
			if (amount        < 0) throw new ArgumentException("amount cannot negative");
			_currentScore -= amount;
			if (_displayedScore > _currentScore) _displayedScore = _currentScore;
			return true;
		}

		public void UpdateGameOverScoreText()
		{
			//save score.
			PlayerPrefs.SetInt("totalKills",   PlayerPrefs.GetInt("totalKills")   + _kills);
			PlayerPrefs.SetInt("totalPickups", PlayerPrefs.GetInt("totalPickups") + _pickups);
			if (PlayerPrefs.GetInt("maxScore") < _totalScore)
				PlayerPrefs.SetInt("maxScore", _totalScore);
			if (PlayerPrefs.GetInt("maxRound") < WaveController.Instance.wave)
				PlayerPrefs.SetInt("maxRound", WaveController.Instance.wave);

			//update UI
			score.transform.parent.gameObject.SetActive(false);
			gameOverScore.text = _totalScore.ToString();
			if (waveText is null) return;
			waveText.enabled = true;
			waveText.color   = new Color(waveText.color.r, waveText.color.g, waveText.color.b, 1);
		}

		public void GivePickupScore() => GiveScore(ScoreType.Pickup);

		private void OnAnyEnemyDeathUpdate(Vector3 position) => GiveScore(ScoreType.EnemyKill, position);

		public static bool GiveScore(ScoreType type) => _instance.ApplyScore(type);

		public static bool GiveScore(ScoreType type, object argument) =>
			type == ScoreType.EnemyKill && argument is Vector3 pos
				? _instance.TextPopUps(pos) & _instance.ApplyScore(type)
				: _instance.ApplyScore(type);

		private bool TextPopUps(Vector3 position)
		{
			ServiceLocator.Pools.SpawnFromPool(PoolType.KillText, position);

			if (_comboTimeRemaining > 0) {
				ServiceLocator.Pools.SpawnFromPool(PoolType.ComboText, position);
				_comboTimeRemaining = comboTimeLimit;
			} else
				StartCoroutine(StartComboTimeRoutine());

			return true;
		}

		private bool ApplyScore(ScoreType type)
		{
			int scoreDelta = 1;
			switch (type) {
				case ScoreType.EnemyKill:
					scoreDelta = killScore * _comboMultiplier;
					_kills++;
					break;
				case ScoreType.Pickup:
					scoreDelta = pickupScore;
					_pickups++;
					break;
			}

			_totalScore += scoreDelta;

			if (_displayedScore == _currentScore) {
				_currentScore += scoreDelta;
				StartCoroutine(UpdateScoreRoutine());
			} else
				_currentScore += scoreDelta;

			if (_comboMultiplier < maxCombo)
				_comboMultiplier += comboAdditionPerKill;

			return true;
		}

		private void UpdateScore(int newScore) => score.text = newScore.ToString();

		private IEnumerator UpdateScoreRoutine()
		{
			while (_displayedScore < _currentScore) {
				UpdateScore(++_displayedScore);
				ServiceLocator.Audio.PlaySFX(scoreTickSFX, 0.04f);
				yield return new WaitForSeconds(0.002f + .75f / (_currentScore - _displayedScore));
			}
		}

		private IEnumerator StartComboTimeRoutine()
		{
			yield return new WaitForEndOfFrame();
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