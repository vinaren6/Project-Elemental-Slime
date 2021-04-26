using _Project.Scripts.Managers;

namespace _Project.Scripts.Enemies.AI
{
    public class EnemyHuntState : EnemyState
    {
        public EnemyHuntState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

        public override void LogicUpdate()
        {
            if (ServiceLocator.Game.IsPaused) {
                _enemy.NavMeshAgent.SetDestination(_transform.position);
                return;
            }

            base.LogicUpdate();
            // Vector3 direction = (_enemy.target.position - _transform.position).normalized;
            // _enemy.Rb.MovePosition(_enemy.Rb.position + direction * (Time.fixedDeltaTime * _enemy.moveSpeed));
            // Quaternion rotation = Quaternion.LookRotation(direction);
            // _transform.rotation = Quaternion.Slerp(
            //     _transform.rotation, rotation, Time.fixedDeltaTime * _enemy.rotationSpeed);
            _enemy.NavMeshAgent.SetDestination(_enemy.Target.position);
        }
    }
}