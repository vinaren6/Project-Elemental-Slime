using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels;

        private void Awake()
        {
            HideAllPanels();
        }

        public void ResumeButtonClick()
        {
            ClosePauseMenu();
        }

        public void RestartButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OptionsButtonClick()
        {
            ShowPanel(PauseMenuPanelType.Options);
        }

        public void BackToMainMenuClick()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void OptionsConfirmClick()
        {
            ShowPanel(PauseMenuPanelType.PauseScreen);
        }

        public void OpenPauseMenu()
        {
            ServiceLocator.Game.SetPause(true);
            ShowPanel(PauseMenuPanelType.PauseScreen);
        }

        public void ClosePauseMenu()
        {
            ServiceLocator.Game.SetPause(false);
            HideAllPanels();
        }
        
        private void HideAllPanels()
        {
            foreach (GameObject panel in panels)
            {
                panel.SetActive(false);
            }
        }

        private void ShowPanel(PauseMenuPanelType panelType)
        {
            HideAllPanels();
            panels[(int) panelType].SetActive(true);
        }
    }
}

