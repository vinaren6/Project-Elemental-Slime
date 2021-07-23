using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace _Project.Scripts.UI.Score.GlobalHighScore
{
	public class HighScores : MonoBehaviour
	{
		private const  string     PrivateCode = "bpQst_tTeUCmPjhrItWQCAFF-_fVz3EEur0vGAqvKhsw";
		private const  string     PublicCode  = "60ca16d68f40bb114c422936";
		private const  string     WebURL      = "http://dreamlo.com/lb/";
		private static HighScores _instance;

		private DisplayHighScores _highScoreDisplay;
		private HighScore[]       _highScoresList;
		private SetUsername       _setUsername;

		private void Awake()
		{
			_highScoreDisplay = GetComponent<DisplayHighScores>();
			_setUsername      = GetComponent<SetUsername>();
			_instance         = this;
		}

		public static void AddNewHighScore()
		{
			string username = PlayerPrefs.GetString("Username", "");
			if (username == "") {
				_instance._setUsername.NewUsername();
				return;
			}

			_instance.StartCoroutine(_instance.UploadNewHighScore(username, PlayerPrefs.GetInt("maxScore")));
		}

		public static bool IsUsernameIsTaken(string username) => _instance.CheckIfUsernameIsTaken(username);

		private bool CheckIfUsernameIsTaken(string username)
		{
			if (_highScoresList is null || _highScoresList.Length <= 0) {
				_setUsername.Error();
				return true;
			}

			if (_highScoresList.Any(highScore => highScore.Username == username)) {
				_setUsername.Error(1);
				return true;
			}

			return false;
		}
		
		public static bool IsUsingAllowedCharacters(string username) =>_instance.CheckUsingAllowedCharacters(username);

		private bool CheckUsingAllowedCharacters(string username)
		{
			if (username.Length > 2 || username.Length < 21 || !Regex.IsMatch(username, @"^[a-zA-Z0-9]+$")) {
				_setUsername.Error(2);
				return true;
			}
			return false;
		}
		

		private IEnumerator UploadNewHighScore(string username, int score)
		{
			UnityWebRequest www = UnityWebRequest.Get(
				WebURL + PrivateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score);
			yield return www.SendWebRequest();

			if (string.IsNullOrEmpty(www.error)) {
				print("Upload Successful");
				DownloadHighScores();
			} else
				print("Error uploading: " + www.error);
		}

		public void DownloadHighScores() => StartCoroutine(nameof(DownloadHighScoresFromDatabase));

		private IEnumerator DownloadHighScoresFromDatabase()
		{
			UnityWebRequest www = UnityWebRequest.Get(WebURL + PublicCode + "/pipe/");
			yield return www.SendWebRequest();

			if (string.IsNullOrEmpty(www.error)) {
				Debug.Log(www.downloadHandler.text);
				FormatHighScores(www.downloadHandler.text);
				_highScoreDisplay.OnHighScoresDownloaded(_highScoresList);
			} else
				print("Error Downloading: " + www.error);
		}

		private void FormatHighScores(string textStream)
		{
			string[] entries = textStream.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
			_highScoresList = new HighScore[entries.Length];

			for (int i = 0; i < entries.Length; i++) {
				string[] entryInfo = entries[i].Split('|');
				string   username  = entryInfo[0];
				int      score     = int.Parse(entryInfo[1]);
				_highScoresList[i] = new HighScore(username, score);
				print(_highScoresList[i].Username + ": " + _highScoresList[i].Score);
			}
		}
	}

	public readonly struct HighScore
	{
		public readonly string Username;
		public readonly int    Score;

		public HighScore(string username, int score)
		{
			Username = username;
			Score    = score;
		}
	}
}