namespace _Project.Scripts.Enemies.AI
{
	public class EnemyDeathState : EnemyState
	{
		public EnemyDeathState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

		public override void Enter() => base.Enter();

		public override void LogicUpdate() => base.LogicUpdate();

		public override void PhysicsUpdate() => base.PhysicsUpdate();

		public override void Exit() => base.Exit();
	}
}