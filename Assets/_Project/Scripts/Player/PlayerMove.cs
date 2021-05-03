using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Player
{
	public class PlayerMove : MonoBehaviour
	{
		private PlayerController _player;
		private NavMeshAgent     _agent;
		private Transform        _transform;
		private Camera           _camera;
		private Vector3          _forward, _right;

		private void Awake()
		{
			_player    = GetComponent<PlayerController>();
			_agent     = GetComponent<NavMeshAgent>();
			_transform = gameObject.transform;
		}
		
		private void Start()
		{
			_camera = Camera.main;
			SetIsometricReferences();
		}

		public void Move(Vector3 input)
		{
			Vector3 moveDirection           = (_forward * input.z + _right * input.x).normalized;
			float   lookDirectionAdjustment = GetLookDirectionAdjustment(moveDirection);
			
			Vector3 movement                = moveDirection * (lookDirectionAdjustment * _player.MoveSpeed);
			AdjustMovementIfPlayerIsAttacking(ref movement);
			_agent.velocity = movement * (1 - _player.MoveSmoothing) + _agent.velocity * _player.MoveSmoothing;
		}

		#region Methods

		private void SetIsometricReferences()
		{
			_forward   = _camera.transform.forward;
			_forward.y = 0;
			_forward   = Vector3.Normalize(_forward);
			_right     = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
		}
		
		private float GetLookDirectionAdjustment(Vector3 moveDirection)
		{
			float moveLookRelation = (Vector3.Dot(_transform.forward, moveDirection) + 1) / 2 ;
			return _player.MoveBackwardsMultiplier + moveLookRelation * (1 - _player.MoveBackwardsMultiplier);
		}
		
		private void AdjustMovementIfPlayerIsAttacking(ref Vector3 movement)
		{
			if (!PlayerController.IsAttacking)
				return;

			movement    *= _player.MoveWhenAttackingMultiplier;
		}
		
		// private void AdjustVelocityIfPlayerHasAttacked()
		// {
		// 	if (!_player.HasAttacked)
		// 		return;
		//
		// 	_agent.velocity     *= _player.MoveWhenAttackingMultiplier;
		// 	_player.HasAttacked =  false;
		// }

		#endregion
	}
}