using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables

        private bool _isPaused;
        
        #region Properties
        
        public bool IsPaused => _isPaused;

        #endregion

        #endregion

        #region Start Methods

        private void Awake()
        {
            ServiceLocator.Initialize(this);
        }

        #endregion

        #region Methods

        public void SetPause(bool pause)
        {
            _isPaused = pause;
        }

        #endregion
    }
}