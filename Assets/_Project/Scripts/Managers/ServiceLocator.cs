using _Project.Scripts.Audio;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public static class ServiceLocator
    {
        #region Variables
        
        private static GameManager _game;
        private static IAudio _audio;

        #region Properties

        public static GameManager Game => _game;
        public static IAudio Audio => _audio;
        
        #endregion

        #endregion

        #region Start Methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void Initialize()
        {
            _game = new GameManager();
            _audio = new NullAudioProvider();
        }

        #endregion

        #region Methods

        #region Provide Setters

        public static void ProvideAudio(IAudio audioService)
        {
            if (audioService == null)
                _audio = new NullAudioProvider();

            _audio = audioService;
        }

        #endregion

        #endregion
    }
}