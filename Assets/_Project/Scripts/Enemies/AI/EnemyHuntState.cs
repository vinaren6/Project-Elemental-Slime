using _Project.Scripts.Managers;

namespace _Project.Scripts.Enemies.AI
{
    public class EnemyHuntState : EnemyState
    {
        public EnemyHuntState(EnemyController enemyController, EnemyStateMachine stateMachine) : base(enemyController, stateMachine) { }

        public override void LogicUpdate()
        {
            if (ServiceLocator.Game.IsPaused) {
                EnemyController.NavMeshAgent.SetDestination(_transform.position);
                return;
            }

            base.LogicUpdate();
            // Vector3 direction = (_enemy.target.position - _transform.position).normalized;
            // _enemy.Rb.MovePosition(_enemy.Rb.position + direction * (Time.fixedDeltaTime * _enemy.moveSpeed));
            // Quaternion rotation = Quaternion.LookRotation(direction);
            // _transform.rotation = Quaternion.Slerp(
            //     _transform.rotation, rotation, Time.fixedDeltaTime * _enemy.rotationSpeed);
            EnemyController.NavMeshAgent.SetDestination(EnemyController.Target.position);
        }
    }
}