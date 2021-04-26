using _Project.Scripts.Events;

namespace _Project.Scripts.Enemies.AI
{
	public class EnemyDeathState : EnemyState
	{
		public EnemyDeathState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

		public override void Enter()
		{
			base.Enter();
			
			_enemy.GetComponent<DestroyThisObj>().DestroyMe();
		}

		public override void LogicUpdate() => base.LogicUpdate();
		
		public override void Exit() => base.Exit();
	}
}