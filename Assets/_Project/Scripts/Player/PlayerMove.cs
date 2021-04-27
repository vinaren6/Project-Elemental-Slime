using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Player
{
	public class PlayerMove : MonoBehaviour
	{
		private Camera           _camera;
		private Vector3          _forward, _right;
		private NavMeshAgent     _agent;
		private Transform        _transform;
		private PlayerController _player;

		private void Awake()
		{
			_agent     = GetComponent<NavMeshAgent>();
			_transform = gameObject.transform;
			_player    = GetComponent<PlayerController>();
		}
		
		private void Start()
		{
			_camera = Camera.main;
			SetIsometricReferences();
		}

		public void Move(Vector3 input, float speed, float moveSmoothing, float backwardsMoveMultiplier, float moveWhenAttackingMultiplier)
		{
			Vector3 moveDirection           = (_forward * input.z + _right * input.x).normalized;
			float   lookDirectionAdjustment = GetLookDirectionAdjustment(moveDirection, backwardsMoveMultiplier);
			
			Vector3 movement                = moveDirection * (lookDirectionAdjustment * speed);
			_agent.velocity = movement * (1 - moveSmoothing) + _agent.velocity * moveSmoothing;

			AdjustVelocityIfPlayerHasAttacked(moveWhenAttackingMultiplier);
		}

		#region Methods

		private void SetIsometricReferences()
		{
			_forward   = _camera.transform.forward;
			_forward.y = 0;
			_forward   = Vector3.Normalize(_forward);
			_right     = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
		}
		
		private float GetLookDirectionAdjustment(Vector3 moveDirection, float backwardsMoveMultiplier)
		{
			float moveLookRelation = (Vector3.Dot(_transform.forward, moveDirection) + 1) / 2 ;
			return backwardsMoveMultiplier + moveLookRelation * (1 - backwardsMoveMultiplier);
		}
		
		private void AdjustVelocityIfPlayerHasAttacked(float moveWhenAttackingMultiplier)
		{
			if (!_player.HasAttacked)
				return;
			
			_agent.velocity     *= moveWhenAttackingMultiplier;
			_player.HasAttacked =  false;
		}

		#endregion
	}
}