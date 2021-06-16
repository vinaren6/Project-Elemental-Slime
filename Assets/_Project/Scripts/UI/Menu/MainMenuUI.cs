using _Project.Scripts.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI.Menu
{
	public class MainMenuUI : MonoBehaviour
	{
		[SerializeField] private GameObject[] panels;
		[SerializeField] private AudioClip clickSFX;

		private void Awake() => ShowPanel(MainMenuPanelType.TitleScreen);

		public void ButtonClick(MainMenuPanelType type) => ShowPanel(type);
			
		public void PlayButtonClick() => SceneManager.LoadScene(1);

		public void OptionsButtonClick() => ShowPanel(MainMenuPanelType.Options);

		public void CreditsButtonClick() => ShowPanel(MainMenuPanelType.Credits);
		
		public void ControlsButtonClick() => ShowPanel(MainMenuPanelType.Controls);

		public void BackToTitleClick() => ShowPanel(MainMenuPanelType.TitleScreen);
		
		public void ScoreboardClick() => ShowPanel(MainMenuPanelType.Scoreboard);

		public void QuitButtonClick()
		{
			#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
			#else
        Application.Quit();
			#endif
		}

		public void StatsButtonClick() => ShowPanel(MainMenuPanelType.Stats);

		private void HideAllPanels()
		{
			foreach (GameObject panel in panels) panel.SetActive(false);
		}

		private void ShowPanel(MainMenuPanelType panelType)
		{
			HideAllPanels();
			panels[(int) panelType].SetActive(true);
		}

		public void PlayClickSound()
		{
			ServiceLocator.Audio.PlaySFX(clickSFX, 0.2f);
		}
	}
}