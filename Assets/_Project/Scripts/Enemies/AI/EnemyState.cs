using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
	public class EnemyState
	{
		protected Enemy             _enemy;
		protected EnemyStateMachine _stateMachine;
		protected Transform         _transform;

		private float _nextDamageOverTime;
		private int _damageOverTimeTicks;

		public EnemyState(Enemy enemy, EnemyStateMachine stateMachine)
		{
			_enemy        = enemy;
			_stateMachine = stateMachine;
			_transform    = enemy.transform;
		}

		public virtual void Enter() { }

		public virtual void LogicUpdate()
		{
			if (!_enemy.IsBurning)
				return;
			
			if (_nextDamageOverTime == 0) {
				_nextDamageOverTime = Time.time + PlayerController.DamageOverTimeCooldownTime;
			}

			if (!(Time.time > _nextDamageOverTime))
				return;
			
			_enemy.Health.ReceiveDamage(ElementalSystemTypes.Fire, PlayerController.PlayerDamageOverTime);
			_nextDamageOverTime = Time.time + PlayerController.DamageOverTimeCooldownTime;
			_damageOverTimeTicks++;

			if (!(_damageOverTimeTicks >= _enemy.damageOverTimeTotalTicks))
				return;

			_nextDamageOverTime = 0f;
			_damageOverTimeTicks = 0;
			_enemy.IsBurning = false;
		}
		
		public virtual void Exit() { }
	}

}