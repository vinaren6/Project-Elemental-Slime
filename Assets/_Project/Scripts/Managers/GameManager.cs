namespace _Project.Scripts.Managers
{
    public class GameManager
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

        public GameManager()
        {
        }

        #endregion

        #region Methods

        public void SetPause(bool pause)
        {
            if (_isPaused == pause) return;
            _isPaused = pause;
            if (OnVariableChange != null)
                OnVariableChange(_isPaused);
        }

        #endregion
    }
}