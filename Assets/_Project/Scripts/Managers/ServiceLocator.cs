using _Project.Scripts.Audio;
using _Project.Scripts.UI;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public static class ServiceLocator
    {
        #region Variables
        
        private static GameManager      _game;
        private static IAudio           _audio;
        private static IHUD             _hud;
        private static ObjectPools      _pools;
        
        #region Properties

        public static GameManager Game  => _game;
        public static IAudio      Audio => _audio;
        public static IHUD        HUD   => _hud;
        public static ObjectPools Pools => _pools;

        #endregion

        #endregion

        #region Start Methods

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        public static void Initialize(GameManager gameManager)
        {
            _game = gameManager;
            _audio ??= new AudioController();
            _hud ??= new NullHUD();
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

        public static void ProvideHUD(IHUD hud)
        {
            if (hud == null)
                _hud = new NullHUD();

            _hud = hud;
        }
        
        public static void ProvideObjectPools(ObjectPools objectPools)
        {
            _pools = objectPools;
        }

        #endregion

        #endregion
    }
}