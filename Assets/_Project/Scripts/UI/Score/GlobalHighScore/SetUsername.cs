using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Score.GlobalHighScore
{
	public class SetUsername : MonoBehaviour
	{
		[SerializeField] private GameObject     newUsernameWindow;
		[SerializeField] private TMP_Text       text;
		[SerializeField] private TMP_InputField inputText;

		public void NewUsername(bool state = true) => newUsernameWindow.SetActive(state);

		public void TryNewUsername()
		{
			if (HighScores.IsUsernameIsTaken(inputText.text)) return;
			PlayerPrefs.SetString("Username", inputText.text);
			HighScores.AddNewHighScore();
			NewUsername(false);
		}

		public void Error(int id = 0) =>
			text.text = id switch {
				0 => "ERROR",
				1 => "Username already taken.",
				_ => text.text
			};
	}
}