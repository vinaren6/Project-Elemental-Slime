namespace _Project.Scripts.Enemies.AI
{
    public class EnemyDetectState : EnemyState
    {
        public EnemyDetectState(EnemyController enemyController, EnemyStateMachine stateMachine) : base(enemyController, stateMachine) { }

        public override void Enter()
        {
            EnemyController.StartPlayerDetection();
            base.Enter();
        } 
    }
}