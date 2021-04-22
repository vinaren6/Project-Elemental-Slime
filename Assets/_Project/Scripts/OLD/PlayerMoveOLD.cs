using UnityEngine;

namespace _Project.Scripts.Player
{
	public class PlayerMoveOLD : MonoBehaviour
	{
		private Camera    _camera;
		private Vector3   _forward, _right;
		private Rigidbody _rb;

		private void Awake() => _rb = GetComponent<Rigidbody>();

		private void Start()
		{
			_camera = Camera.main;
			SetIsometricReferences();
		}

		private void SetIsometricReferences()
		{
			_forward   = _camera.transform.forward;
			_forward.y = 0;
			_forward   = Vector3.Normalize(_forward);
			_right     = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
		}

		public void Move(Vector3 input, float speed)
		{
			Vector3 moveDirection = ((_forward * input.z) + (_right * input.x)).normalized;
			_rb.velocity = moveDirection * speed;
			
			// VelocityMovement with smoothing:
			// _rb.velocity = (moveDirection * (0.2f * speed)) + _rb.velocity * 0.8f;
		}
	}
}