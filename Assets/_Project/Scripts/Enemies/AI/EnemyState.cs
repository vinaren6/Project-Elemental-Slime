using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
	public class EnemyState
	{
		protected EnemyController             EnemyController;
		protected EnemyStateMachine _stateMachine;
		protected Transform         _transform;

		private Vector3 _direction;
		private Quaternion _lookRotation;

		private float _rotationSpeed;
		private float _nextDamageOverTime;
		private int _damageOverTimeTicks;

		public EnemyState(EnemyController enemyController, EnemyStateMachine stateMachine)
		{
			EnemyController        = enemyController;
			_stateMachine = stateMachine;
			_transform    = enemyController.transform;

			_rotationSpeed = 10f;
		}

		public virtual void Enter()
		{
			// Debug.Log($"{_transform.name} Enter {_stateMachine.CurrentState} state.");
		}

		public virtual void LogicUpdate()
		{
			EnemyController.Animator.SetBool("IsMoving", EnemyController.NavMeshAgent.velocity.magnitude > 0.1f);
			
			_direction = (EnemyController.Target.position - _transform.position).normalized;
			_lookRotation = Quaternion.LookRotation(_direction);
			_transform.rotation = Quaternion.Slerp(_transform.rotation, _lookRotation, _rotationSpeed * Time.fixedDeltaTime);
			
			EnemyController.NavMeshAgent.updateRotation = false;
			// EnemyController.NavMeshAgent.updatePosition = false;

			//float speed = 6f * EnemyController.CurrentEnemyElementalStats.moveSpeedMultiplier;
			
			// EnemyController.NavMeshAgent.Move(_transform.forward * (speed * Time.deltaTime));
			// EnemyController.NavMeshAgent.velocity = _transform.forward * (speed * Time.deltaTime);
			
			if (!EnemyController.IsBurning)
				return;
			
			if (_nextDamageOverTime == 0) {
				_nextDamageOverTime = Time.fixedTime + PlayerController.DamageOverTimeCooldownTime;
			}

			if (!(Time.fixedTime > _nextDamageOverTime))
				return;
			
			EnemyController.Health.ReceiveDamage(ElementalSystemTypes.Fire, PlayerController.PlayerDamageOverTime);
			_nextDamageOverTime = Time.fixedTime + PlayerController.DamageOverTimeCooldownTime;
			_damageOverTimeTicks++;

			if (!(_damageOverTimeTicks >= EnemyController.DamageOverTimeTotalTicks))
				return;

			_nextDamageOverTime = 0f;
			_damageOverTimeTicks = 0;
			EnemyController.IsBurning = false;
		}

		public virtual void Exit() { }
	}

}