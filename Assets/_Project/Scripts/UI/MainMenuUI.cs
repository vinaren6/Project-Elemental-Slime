using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels;

        private void Awake()
        {
            HideAllPanels();
            ShowPanel(MainMenuPanelType.TitleScreen);
        }

        public void PlayButtonClick()
        {
            SceneManager.LoadScene("TestSceneLinus");
        }

        public void OptionsButtonClick()
        {
            HideAllPanels();
            ShowPanel(MainMenuPanelType.Options);
        }

        public void CreditsButtonClick()
        {
            HideAllPanels();
            ShowPanel(MainMenuPanelType.Credits);
        }

        public void BackToTitleClick()
        {
            HideAllPanels();
            ShowPanel(MainMenuPanelType.TitleScreen);
        }

        public void QuitButtonClick()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        private void HideAllPanels()
        {
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }
        }

        private void ShowPanel(MainMenuPanelType panelType)
        {
            panels[(int) panelType].SetActive(true);
        }
    }
}
