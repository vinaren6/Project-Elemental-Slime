using System;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        private bool _isPaused;
        public delegate void OnVariableChangeDelegate(bool state);
        public event OnVariableChangeDelegate OnVariableChange;

        #region Properties

        public bool IsPaused => _isPaused;


        #endregion

        #endregion

        #region Start Methods

        private void Awake()
        {
            Screen.SetResolution(1920, 1080, true);
            ServiceLocator.Initialize(this);
        }

        #endregion

        #region Methods

        public void SetPause(bool pause)
        {
            if (_isPaused == pause) return;
            _isPaused = pause;

            Time.timeScale = pause ? 0f : 1f;
            
            if (OnVariableChange != null)
                OnVariableChange(_isPaused);
        }

        public bool GetPause() => _isPaused;

#endregion
    }
}