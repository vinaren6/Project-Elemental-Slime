using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Player
{
	public class PlayerMove : MonoBehaviour
	{
		private Camera       _camera;
		private Vector3      _forward, _right;
		private NavMeshAgent _agent;
		private Transform    _transform;

		private void Awake()
		{
			_agent     = GetComponent<NavMeshAgent>();
			_transform = gameObject.transform;
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

		public void Move(Vector3 input, float speed, float backwardsMoveMultiplier)
		{
			Vector3 moveDirection           = (_forward * input.z + _right * input.x).normalized;
			float   lookDirectionAdjustment = GetLookDirectionAdjustment(moveDirection, backwardsMoveMultiplier);

			// VelocityMovement with smoothing:
			_agent.velocity = moveDirection * (Time.deltaTime * speed * lookDirectionAdjustment * 100 * 0.1f) + _agent.velocity * 0.9f;
			
			// -- MOVEMENT ALTERNATIVES --
			// _agent.Move(moveDirection * (Time.deltaTime * speed * lookDirectionAdjustment));
			// VelocityMovement:
			// _agent.velocity = moveDirection * (Time.deltaTime * speed * 100);
		}

		private float GetLookDirectionAdjustment(Vector3 moveDirection, float backwardsMoveMultiplier)
		{
			Vector3 lookDirection = _transform.forward;
			float lookDirectionAdjustment = (Mathf.Abs(moveDirection.x + lookDirection.x) + Mathf.Abs(moveDirection.z + lookDirection.z)) / 2;
			lookDirectionAdjustment = backwardsMoveMultiplier + Mathf.Clamp(lookDirectionAdjustment, 0.1f, 1f) * (1 - backwardsMoveMultiplier);
			return lookDirectionAdjustment;
		}
	}
}