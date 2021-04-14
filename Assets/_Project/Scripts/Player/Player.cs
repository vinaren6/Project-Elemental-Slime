using System;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class Player : MonoBehaviour
	{
		[Header("PLAYER SETTINGS:")]
		[SerializeField] private float movementSpeed = 0.2f;
		
		private PlayerInputHandler _inputHandler;
		private PlayerAim          _playerAim;
		private Rigidbody          _rb;
		private Camera             _camera;
		private Vector3            _forward, _right;

		private void Awake()
		{
			_inputHandler = GetComponent<PlayerInputHandler>();
			_playerAim    = GetComponent<PlayerAim>();
			_rb           = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			_camera = Camera.main;
			SetIsometricReferences();
		}

		private void Update()
		{
			_playerAim.AimUpdate(_inputHandler.AimDirection, _camera);	
		}

		private void FixedUpdate()
		{
			MovePlayer();
		}

		private void SetIsometricReferences()
		{
			_forward   = _camera.transform.forward;
			_forward.y = 0;
			_forward   = Vector3.Normalize(_forward);
			_right     = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
		}
		
		private void MovePlayer()
		{
			if (_inputHandler.MoveDirection == Vector3.zero)
				return;
			
			Vector3 upMovement        = _forward * _inputHandler.MoveDirection.z;
			Vector3 rightMovement     = _right   * _inputHandler.MoveDirection.x;
			Vector3 movementDirection = (upMovement + rightMovement).normalized;
			_rb.MovePosition(_rb.position + movementDirection * movementSpeed);
		}
	}
}