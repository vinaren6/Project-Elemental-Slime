using System.Diagnostics;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Score.GlobalHighscore
{
	public class SetUsername : MonoBehaviour
	{
		[SerializeField] private GameObject     newUsernameWindow;
		[SerializeField] private TMP_Text       text;
		[SerializeField] private TMP_InputField inputText;

		public void NewUsername(bool state = true) => newUsernameWindow.SetActive(state);

		public void TryNewUsername()
		{
			if (Highscores.IsUsernameIsTaken(inputText.text)) return;
			PlayerPrefs.SetString("Username", inputText.text);
			Highscores.AddNewHighscore();
			NewUsername(false);
		}

		public void Error(int id = 0)
		{
			switch (id) {
				case 0:
					text.text = "ERROR";
					break;
				case 1:
					text.text = "Username already taken.";
					break;
			}
		}
	}
}