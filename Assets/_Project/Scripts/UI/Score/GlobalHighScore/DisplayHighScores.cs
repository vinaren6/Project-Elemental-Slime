using System.Collections;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Score.GlobalHighScore
{
	public class DisplayHighScores : MonoBehaviour
	{
		public  TMP_Text[] highScoreFields;
		private HighScores _highScoresManager;

		private void Start()
		{
			for (int i = 0; i < highScoreFields.Length; i++) highScoreFields[i].text = i + 1 + ". Fetching...";

			_highScoresManager = GetComponent<HighScores>();
			StartCoroutine(nameof(RefreshHighScores));
		}

		public void OnHighScoresDownloaded(HighScore[] highScoreList)
		{
			for (int i = 0; i < highScoreFields.Length; i++) {
				highScoreFields[i].text = i + 1 + ". ";
				if (i < highScoreList.Length)
					highScoreFields[i].text += highScoreList[i].Username + " - " +
					                           StatTextDisplayInt.ToString(highScoreList[i].Score);
			}
		}

		private IEnumerator RefreshHighScores()
		{
			while (true) {
				_highScoresManager.DownloadHighScores();
				yield return new WaitForSeconds(30);
			}
		}
	}
}