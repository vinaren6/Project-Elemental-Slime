using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("PLAYER SETTINGS:")] [SerializeField]
        private float movementSpeed = 0.2f;

        private Camera _camera;

        private PlayerInputHandler _inputHandler;
        private PlayerAim _playerAim;
        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _inputHandler = GetComponent<PlayerInputHandler>();
            _playerAim = GetComponent<PlayerAim>();
            _playerMovement = GetComponent<PlayerMovement>();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            _playerAim.AimUpdate(_inputHandler.AimDirection, _camera);
        }

        private void FixedUpdate()
        {
            _playerMovement.Move(_inputHandler, movementSpeed);
        }
    }
}