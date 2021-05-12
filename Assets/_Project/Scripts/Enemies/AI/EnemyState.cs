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

		private float _nextDamageOverTime;
		private int _damageOverTimeTicks;

		public EnemyState(EnemyController enemyController, EnemyStateMachine stateMachine)
		{
			EnemyController        = enemyController;
			_stateMachine = stateMachine;
			_transform    = enemyController.transform;
		}

		public virtual void Enter()
		{
			Debug.Log($"{_transform.name} Enter {_stateMachine.CurrentState} state.");
		}

		public virtual void LogicUpdate()
		{
			EnemyController.Animator.SetBool("IsMoving", EnemyController.NavMeshAgent.velocity.magnitude > 0.1f);

			// EnemyController.NavMeshAgent.velocity = _transform.forward * (120f * Time.deltaTime);
			// _transform.rotation = Quaternion.LookRotation(EnemyController.Target.position);

			Transform lookTransform = _transform;
			lookTransform.LookAt(EnemyController.Target.position, Vector3.up);
			
			// _transform.LookAt(EnemyController.Target.position, Vector3.up);

			float speed = 6f * EnemyController.CurrentEnemyElementalStats.moveSpeedMultiplier;
			
			_transform.eulerAngles =
				Vector3.RotateTowards(_transform.eulerAngles,
					lookTransform.eulerAngles,
					5f,
					5f);
			EnemyController.NavMeshAgent.Move(_transform.forward * (speed * Time.deltaTime));
			
			if (!EnemyController.IsBurning)
				return;
			
			if (_nextDamageOverTime == 0) {
				_nextDamageOverTime = Time.time + PlayerController.DamageOverTimeCooldownTime;
			}

			if (!(Time.time > _nextDamageOverTime))
				return;
			
			EnemyController.Health.ReceiveDamage(ElementalSystemTypes.Fire, PlayerController.PlayerDamageOverTime);
			_nextDamageOverTime = Time.time + PlayerController.DamageOverTimeCooldownTime;
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