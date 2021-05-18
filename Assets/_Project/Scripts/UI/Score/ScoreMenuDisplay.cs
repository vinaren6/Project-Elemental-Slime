using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Score
{
	public class ScoreMenuDisplay : MonoBehaviour
	{
		public TMP_Text maxRound, maxScore, totalKills, totalPickups;

		private void Start()
		{
			maxRound.text     = PlayerPrefs.GetInt("maxRound").ToString();
			maxScore.text     = PlayerPrefs.GetInt("maxScore").ToString();
			totalKills.text   = PlayerPrefs.GetInt("totalKills").ToString();
			totalPickups.text = PlayerPrefs.GetInt("totalPickups").ToString();
		}
	}
}