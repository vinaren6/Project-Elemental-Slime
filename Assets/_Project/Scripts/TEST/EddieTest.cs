using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.TEST
{
    public class EddieTest : MonoBehaviour
    {
        private void Awake()
        {
            ServiceLocator.Initialize();
        }

        private void Start()
        {
            // Debug.Log($"Game.IsPaused: {ServiceLocator.Game.IsPaused}");
        }
    }
}
