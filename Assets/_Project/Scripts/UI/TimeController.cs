using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class TimeController : MonoBehaviour
	{
		[SerializeField] private TMP_Text timeText;
		[SerializeField] private Health   playerHealth;

		[SerializeField] private float timeLimit = 90f;

		private float timeRemaining;

		private void Start()
		{
			timeRemaining = timeLimit;
		}

		private void Update()
		{
			if (timeRemaining > 0) {
				timeRemaining -= Time.deltaTime;
				timeText.text =  Mathf.CeilToInt(timeRemaining).ToString();
			} else {
				ServiceLocator.Game.SetPause(true);
				playerHealth.KillPlayer();
				Destroy(gameObject);
			}
		}
	}
}