using System;
using UnityEngine;
using AudioType = _Project.Scripts.Audio.AudioType;

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
            
            if (pause) {
                Time.timeScale = 0f;
                ServiceLocator.Audio.Mute(AudioType.SFX, true);
            }            
            else {
                Time.timeScale = 1f;
                ServiceLocator.Audio.Mute(AudioType.SFX, false);
            }
            
            if (OnVariableChange != null)
                OnVariableChange(_isPaused);
        }

        public bool GetPause() => _isPaused;

#endregion
    }
}