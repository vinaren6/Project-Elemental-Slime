using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Score.GlobalHighscore
{
	public class DisplayHighscores : MonoBehaviour
	{
		public TMP_Text[] highscoreFields;
		Highscores        _highscoresManager;

		void Start()
		{
			for (int i = 0; i < highscoreFields.Length; i++) { highscoreFields[i].text = i + 1 + ". Fetching..."; }

			_highscoresManager = GetComponent<Highscores>();
			StartCoroutine("RefreshHighscores");
		}

		public void OnHighscoresDownloaded(Highscore[] highscoreList)
		{
			for (int i = 0; i < highscoreFields.Length; i++) {
				highscoreFields[i].text = i + 1 + ". ";
				if (i < highscoreList.Length) {
					highscoreFields[i].text += highscoreList[i].username + " - " + StatTextDisplayInt.ToString(highscoreList[i].score);
				}
			}
		}

		IEnumerator RefreshHighscores()
		{
			while (true) {
				_highscoresManager.DownloadHighscores();
				yield return new WaitForSeconds(30);
			}
		}
	}
}