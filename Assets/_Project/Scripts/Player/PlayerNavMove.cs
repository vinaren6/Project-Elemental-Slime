using System;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Player
{
	public class PlayerNavMove : MonoBehaviour
	{
		private Camera       _camera;
		private Vector3      _forward, _right;
		private NavMeshAgent _agent;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
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

		public void Move(Vector3 input, float speed)
		{
			Vector3 moveDirection = ((_forward * input.z) + (_right * input.x)).normalized;
			_agent.Move(moveDirection * (Time.deltaTime * speed));
			
			// Movement Alternatives:
			// _agent.velocity = moveDirection * (Time.deltaTime * speed * 100);
			// _agent.velocity = moveDirection * (Time.deltaTime * speed * 100 * 0.15f) + _agent.velocity * 0.85f;
		}
	}
}