using _Project.Scripts.HealthSystem;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
	public class EnemyState
	{
		protected Enemy             _enemy;
		protected EnemyStateMachine _stateMachine;
		protected Transform         _transform;

		private float  _nextDamageOverTime = 0;

		public EnemyState(Enemy enemy, EnemyStateMachine stateMachine)
		{
			_enemy        = enemy;
			_stateMachine = stateMachine;
			_transform    = enemy.transform;
		}

		public virtual void Enter() { }

		public virtual void LogicUpdate()
		{
			if (!_enemy.IsBurning) return;
			if (Time.time > _nextDamageOverTime) {
				_enemy.health.ReceiveDamage(_enemy.type.Type, PlayerController.EnemyDamage * PlayerController.DamageOverTimeMultiplier);
				_nextDamageOverTime = Time.time + PlayerController.DamageOverTimeRate;
			}
		}

		public virtual void PhysicsUpdate() { }

		public virtual void Exit() { }
	}

}