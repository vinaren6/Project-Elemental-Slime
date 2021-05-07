using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

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

        private void Update()
        {
            if (Keyboard.current.kKey.wasPressedThisFrame)
                GameObject.FindWithTag("Player").GetComponent<IHealth>().ReceiveDamage(ElementalSystemTypes.Earth, 20f);
        }
    }
}
