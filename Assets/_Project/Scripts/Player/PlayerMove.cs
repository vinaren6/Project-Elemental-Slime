using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Player
{
	public class PlayerMove : MonoBehaviour
	{
		[SerializeField] private float moveAimDifferenceRate = 1f;
		
		private Camera       _camera;
		private Vector3      _forward, _right;
		private NavMeshAgent _agent;

		private void Awake()
		{
			_agent     = GetComponent<NavMeshAgent>();
		}
		
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

		public void Move(Vector3 input, float speed, Vector3 aimDirection)
		{
			Vector3 moveDirection = (_forward * input.z + _right * input.x).normalized;
			
			float moveAimDifference = Mathf.Abs(moveDirection.x + aimDirection.x) + Mathf.Abs(moveDirection.z + aimDirection.z);
			moveAimDifference = Mathf.Clamp(moveAimDifference, 0.5f, 1.5f) * moveAimDifferenceRate;
			// print(moveAimDifference);
			
			// VelocityMovement with smoothing:
			_agent.velocity = moveDirection * (Time.deltaTime * speed * moveAimDifference * 300 * 0.1f) + _agent.velocity * 0.9f;
			
			// -- MOVEMENT ALTERNATIVES --
			// _agent.Move(moveDirection * (Time.deltaTime * speed));
			// VelocityMovement:
			// _agent.velocity = moveDirection * (Time.deltaTime * speed * 100);
		}
	}
}