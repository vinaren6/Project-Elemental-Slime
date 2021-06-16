using System.Collections;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.UI.Score.GlobalHighscore
{
	public class Highscores : MonoBehaviour
	{
		private const string PrivateCode = "bpQst_tTeUCmPjhrItWQCAFF-_fVz3EEur0vGAqvKhsw";
		private const string PublicCode  = "60ca16d68f40bb114c422936";
		private const string WebURL      = "http://dreamlo.com/lb/";

		private        DisplayHighscores _highscoreDisplay;
		private        SetUsername       _setUsername;
		private        Highscore[]       _highscoresList;
		private static Highscores        _instance;

		void Awake()
		{
			_highscoreDisplay = GetComponent<DisplayHighscores>();
			_setUsername      = GetComponent<SetUsername>();
			_instance         = this;
		}

		public static void AddNewHighscore()
		{
			string username = PlayerPrefs.GetString("Username", "");
			if (username == "") {
				_instance._setUsername.NewUsername();
				return;
			}
			_instance.StartCoroutine(_instance.UploadNewHighscore(username, PlayerPrefs.GetInt("maxScore")));
		}

		public static bool IsUsernameIsTaken(string username) => _instance.CheckIfUsernameIsTaken(username);
		
		private bool CheckIfUsernameIsTaken(string username)
		{
			if (_highscoresList is null || _highscoresList.Length <= 0) {
				_setUsername.Error();
				return true;
			}

			foreach (Highscore highscore in _highscoresList) {
				if (highscore.username == username) {
					_setUsername.Error(1);
					return true;
				}
			}

			return false;
		}

		IEnumerator UploadNewHighscore(string username, int score)
		{
			WWW www = new WWW(WebURL + PrivateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
			yield return www;

			if (string.IsNullOrEmpty(www.error)) {
				print("Upload Successful");
				DownloadHighscores();
			} else { print("Error uploading: " + www.error); }
		}

		public void DownloadHighscores() { StartCoroutine("DownloadHighscoresFromDatabase"); }

		IEnumerator DownloadHighscoresFromDatabase()
		{
			WWW www = new WWW(WebURL + PublicCode + "/pipe/");
			yield return www;

			if (string.IsNullOrEmpty(www.error)) {
				FormatHighscores(www.text);
				_highscoreDisplay.OnHighscoresDownloaded(_highscoresList);
			} else { print("Error Downloading: " + www.error); }
		}

		void FormatHighscores(string textStream)
		{
			string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
			_highscoresList = new Highscore[entries.Length];

			for (int i = 0; i < entries.Length; i++) {
				string[] entryInfo = entries[i].Split(new char[] {'|'});
				string   username  = entryInfo[0];
				int      score     = int.Parse(entryInfo[1]);
				_highscoresList[i] = new Highscore(username, score);
				//print(_highscoresList[i].username + ": " + _highscoresList[i].score);
			}
		}
	}

	public struct Highscore
	{
		public string username;
		public int    score;

		public Highscore(string username, int score)
		{
			this.username = username;
			this.score    = score;
		}
	}
}