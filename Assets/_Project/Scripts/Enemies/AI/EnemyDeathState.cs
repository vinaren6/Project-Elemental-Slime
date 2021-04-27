using _Project.Scripts.Events;

namespace _Project.Scripts.Enemies.AI
{
	public class EnemyDeathState : EnemyState
	{
		public EnemyDeathState(EnemyController enemyController, EnemyStateMachine stateMachine) : base(enemyController, stateMachine) { }

		public override void Enter() => base.Enter();

		public override void LogicUpdate()
		{
			base.LogicUpdate();
			EnemyController.GetComponent<DestroyThisObj>().DestroyMe();
		}

		public override void Exit() => base.Exit();
	}
}