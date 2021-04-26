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
    }
}