using System.Collections;
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

		public  int   killScore            = 10;
		public  int   comboAdditionPerKill = 1;
		public  float comboTimeLimit       = 25f;
		
		private int   _currentScore    = 0;
		private int   _newScore    = 0;
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
				
		private void OnAnyDeathUpdate(Vector3 position)
		{
			killFeedbackPool.SpawnKillTextFromPool(position);
			killFeedbackPool.SpawnComboTextFromPool(position);
			
			if (_currentScore == _newScore) {
				_newScore = _currentScore + killScore * _comboMultiplier;
				StartCoroutine(UpdateScoreRoutine());
			} 
			else {
				_newScore = _currentScore + killScore * _comboMultiplier;	
			}

			_comboMultiplier += comboAdditionPerKill;

			if (_comboTimeRemaining > 0)
				_comboTimeRemaining =  comboTimeLimit;
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
			while (_comboTimeRemaining > 0)
			{
				_comboTimeRemaining -= Time.deltaTime;
				// print(comboTimeRemaining);
				yield return null;
			}

			_comboMultiplier = 1;
			_comboIsActive   = false;
		}
	}
}