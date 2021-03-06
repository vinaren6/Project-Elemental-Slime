using System;
using System.Collections;
using _Project.Scripts.Events.Stats;
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

		[SerializeField] private InGameUI     inGameUI;
		[SerializeField] private Animator     comboChainAnimator;
		[SerializeField] private AudioClip    scoreTickSFX;
		[SerializeField] private AudioClip    comboSFX;
		[SerializeField] private ComboMeterUI comboMeterUI;
		[SerializeField] private TMP_Text     waveText;
		[SerializeField] private TMP_Text     scoreText;
		[SerializeField] private TMP_Text     score;
		[SerializeField] private TMP_Text     comboChainText;
		[SerializeField] private TMP_Text     gameOverScore;

		public int   killScore             = 10;
		public int   pickupScore           = 5;
		public float scoreAdditionPerCombo = 0.1f;
		public float comboTimeLimit        = 25f;
		
		private int   _currentCombo;
		private float _comboTimeRemaining;

		private        int  _displayedScore;
		private        int  _currentScore;
		private        int  _totalScore;
		private        int  _kills;
		private        int  _pickups;
		private static bool _isBetweenRounds;

		private void Awake()
		{
			_instance              =  this;
			scoreText.font         =  inGameUI.inGameFont;
			score.font             =  inGameUI.inGameFont;
			score.text             =  _displayedScore.ToString();
			UpdateComboChainText();
		}

		/// <summary>
		///     Attempt to spend a amount of score.
		/// </summary>
		/// <param name="amount">The amount must be a positive number.</param>
		/// <returns>Returns if the spending was successful or not.</returns>
		public bool SpendScore(int amount)
		{
			if (_currentScore < amount) return false;
			if (amount        < 0) throw new ArgumentException("amount cannot be negative");
			_currentScore -= amount;
			if (_displayedScore > _currentScore) _displayedScore = _currentScore;
			return true;
		}

		public void UpdateGameOverScoreText()
		{
			//save score.
			UpdateStat.AddStat("games", 1);
			UpdateStat.SetMaxAndAddStat("maxKills", "totalKills",   _kills);
			UpdateStat.SetMaxAndAddStat("maxPickups", "totalPickups", _pickups);
			UpdateStat.SetMaxAndAddStat("maxScore", "totalScore",   _totalScore);
			UpdateStat.SetMaxAndAddStat("maxRound", "totalRound",   WaveController.Instance.wave);

			//update UI
			score.transform.parent.gameObject.SetActive(false);
			gameOverScore.text = _totalScore.ToString();
		}

		public void GivePickupScore() => ApplyScore(ScoreType.Pickup);
		
		public static bool GiveScore(ScoreType type) => _instance.ApplyScore(type);

		public static bool GiveScore(ScoreType type, Vector3 pos) =>
			type == ScoreType.EnemyKill
				? _instance.TextPopUps(pos) & _instance.ApplyScore(type) & _instance.UpdateCombo()
				: _instance.ApplyScore(type);
		
		private bool TextPopUps(Vector3 position)
		{
			ServiceLocator.Pools.SpawnFromPool(PoolType.KillText, position);

			if (_comboTimeRemaining > 0)
				ServiceLocator.Pools.SpawnFromPool(PoolType.ComboText, position);
				
			return true;
		}

		private bool ApplyScore(ScoreType type)
		{
			int scoreDelta = 1;
			switch (type) {
				case ScoreType.EnemyKill:
					scoreDelta = (int) (killScore + Mathf.Clamp(_currentCombo * scoreAdditionPerCombo * killScore, 0, killScore));
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
			} 
			else {
				_currentScore += scoreDelta;
			}
			
			return true;
		}
		
		private bool UpdateCombo()
		{
			if (_comboTimeRemaining > 0) {
				_comboTimeRemaining = comboTimeLimit;
				_currentCombo++;
				UpdateComboChainText();
			} 
			else {
				StartCoroutine(StartComboTimeRoutine());
			}

			return true;
		}

		private void UpdateScore(int newScore) => score.text = newScore.ToString();

		private IEnumerator UpdateScoreRoutine()
		{
			while (_displayedScore < _currentScore) {
				UpdateScore(++_displayedScore);
				ServiceLocator.Audio.PlaySFX(scoreTickSFX, 0.04f);
				yield return new WaitForSeconds(0.01f + .25f / (_currentScore - _displayedScore));
			}
		}

		private IEnumerator StartComboTimeRoutine()
		{
			yield return new WaitForEndOfFrame();
			_comboTimeRemaining = comboTimeLimit;
			_currentCombo++;

			while (_comboTimeRemaining > 0) {
				if (_isBetweenRounds)
					comboChainAnimator.Play("ComboChain", -1, 0f);
				else
					_comboTimeRemaining -= Time.deltaTime;
				
				comboMeterUI.UpdateUI(_comboTimeRemaining / comboTimeLimit);
				yield return null;
			}

			comboMeterUI.UpdateUI(0f);
			_currentCombo  = 0;
			UpdateComboChainText();
		}

		public static void IsBetweenRounds(bool isTrue) => _isBetweenRounds = isTrue;
		
		private void UpdateComboChainText()
		{
			if (_currentCombo > 1) {
				string chainNumber = _currentCombo.ToString();
				comboChainText.text = chainNumber + "-CHAIN";
				comboChainAnimator.Play("ComboChain", -1, 0f);
				ServiceLocator.Audio.PlaySFX(comboSFX, 0.03f);
			} 
			else {
				comboChainText.text = "";
			}
		}
	}
}