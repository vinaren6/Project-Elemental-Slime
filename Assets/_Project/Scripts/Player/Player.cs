using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class Player : MonoBehaviour
	{
		[Header("PLAYER SETTINGS:")]
		[SerializeField] private float movementSpeed = 1.0f;
		
		private Rigidbody _rb;
		private float     _verticalInput;
		private float     _horizontalInput;
		private Camera    _mainCamera;
		private Vector3   _forward, _right;

		private void Start()
		{
			_rb         = GetComponent<Rigidbody>();
			_mainCamera = Camera.main;
			SetIsometricReferences();
		}

		private void FixedUpdate()
		{
			GetInput();
			MovePlayer();
		}

		private void SetIsometricReferences()
		{
			_forward   = _mainCamera.transform.forward;
			_forward.y = 0;
			_forward   = Vector3.Normalize(_forward);
			_right     = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
		}

		private void GetInput()
		{
			_horizontalInput = Input.GetAxisRaw("Horizontal");
			_verticalInput   = Input.GetAxisRaw("Vertical");
		}

		private void MovePlayer()
		{
			if (_horizontalInput == 0 && _verticalInput == 0)
				return;
			Vector3 upMovement        = _forward * _verticalInput;
			Vector3 rightMovement     = _right   * _horizontalInput;
			Vector3 movementDirection = (upMovement + rightMovement).normalized;
			_rb.MovePosition(_rb.position + movementDirection * (Time.fixedDeltaTime * movementSpeed));
		}
	}
}