using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
	public class EnemyState
	{
		protected Enemy             _enemy;
		protected EnemyStateMachine _stateMachine;
		protected Transform         _transform;

		public EnemyState(Enemy enemy, EnemyStateMachine stateMachine)
		{
			_enemy        = enemy;
			_stateMachine = stateMachine;
			_transform    = enemy.transform;
		}

		public virtual void Enter() { }

		public virtual void LogicUpdate() { }

		public virtual void PhysicsUpdate() { }

		public virtual void Exit() { }
	}

}