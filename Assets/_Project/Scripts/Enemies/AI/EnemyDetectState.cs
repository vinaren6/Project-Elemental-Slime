namespace _Project.Scripts.Enemies.AI
{
    public class EnemyDetectState : EnemyState
    {
        public EnemyDetectState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            _enemy.StartPlayerDetection();
        }

        public override void LogicUpdate() => base.LogicUpdate();

        public override void PhysicsUpdate() => base.PhysicsUpdate();

        public override void Exit() => base.Exit();
    }
}