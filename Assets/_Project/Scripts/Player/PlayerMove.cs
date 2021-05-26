using System.Collections;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Player
{
	public class PlayerMove : MonoBehaviour
	{
		[SerializeField] private AudioClip moveSFX;
		
		private PlayerController _player;
		private NavMeshAgent     _agent;
		private Transform        _transform;
		private Camera           _camera;
		private Vector3          _forward, _right;
		private bool _canPlayMoveSound;
		private float _soundDelay;
		private AudioSource _audioSource;

		private void Awake()
		{
			_player    = GetComponent<PlayerController>();
			_agent     = GetComponent<NavMeshAgent>();
			_transform = gameObject.transform;
			_canPlayMoveSound = true;
			_soundDelay = moveSFX.length + 0.35f;
			_audioSource = gameObject.AddComponent<AudioSource>();
			_audioSource.clip = moveSFX;
			_audioSource.volume = 0.005f;
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
			_player.Animator.SetBool("IsMoving", movement.magnitude > 0.1f);
			_agent.velocity = movement * (1 - _player.MoveSmoothing) + _agent.velocity * _player.MoveSmoothing;
			
			// Debug.Log($"speed?= {_agent.velocity.sqrMagnitude.ToString("F4")}");
			if (_canPlayMoveSound && _agent.velocity.sqrMagnitude > 0.1f)
				StartCoroutine(nameof(MoveSoundRoutine));
			else if (_agent.velocity.sqrMagnitude < 0.1f)
				_audioSource.Stop();
		}

		private IEnumerator MoveSoundRoutine()
		{
			// ServiceLocator.Audio.PlaySFX(moveSFX, 0.025f);
			_audioSource.Play();
			_canPlayMoveSound = false;
			yield return new WaitForSeconds(_soundDelay);
			_canPlayMoveSound = true;
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