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