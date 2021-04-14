namespace _Project.Scripts.Managers
{
    public class GameManager
    {
        #region Variables

        private bool _isPaused;

        #region Properties

        public bool IsPaused => _isPaused;

        #endregion

        #endregion

        #region Start Methods

        public GameManager()
        {
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