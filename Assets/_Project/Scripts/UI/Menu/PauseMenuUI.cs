using System;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI.Menu
{
	public class PauseMenuUI : MonoBehaviour
	{
		[SerializeField] private InputActionAsset controls;
		[SerializeField] private GameObject[]     panels;

		private InputAction _inputAction;

		private void Awake()
		{
			_inputAction = controls.FindActionMap("UI").FindAction("Pause");
			_inputAction.performed += OnInputActionPerformed;
			HideAllPanels();
		}

		private void OnInputActionPerformed(InputAction.CallbackContext _)
		{
			if (!ServiceLocator.Game.IsPaused)
				OpenPauseMenu();
			else
				ClosePauseMenu();
		}

		private void OnDestroy() => _inputAction.performed -= OnInputActionPerformed;

		private void OnEnable()  => _inputAction.Enable();
		private void OnDisable() => _inputAction.Disable();

		public void ResumeButtonClick() => ClosePauseMenu();

		public void RestartButtonClick()
		{
			ServiceLocator.Game.SetPause(false);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void OptionsButtonClick() => ShowPanel(PauseMenuPanelType.Options);
		public void ControlsButtonClick() => ShowPanel(PauseMenuPanelType.Controls);

		public void BackToMainMenuClick()
		{
			ServiceLocator.Game.SetPause(false);
			SceneManager.LoadScene(0);
		}

		public void OptionsConfirmClick() => ShowPanel(PauseMenuPanelType.PauseScreen);

		private void OpenPauseMenu()
		{
			ServiceLocator.Game.SetPause(true);
			ShowPanel(PauseMenuPanelType.PauseScreen);
		}

		private void ClosePauseMenu()
		{
			ServiceLocator.Game.SetPause(false);
			HideAllPanels();
		}

		private void HideAllPanels()
		{
			foreach (GameObject panel in panels) {
				if (panel.activeSelf)
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