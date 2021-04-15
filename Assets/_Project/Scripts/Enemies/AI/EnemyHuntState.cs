using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
    public class EnemyHuntState : EnemyState
    {
        public EnemyHuntState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

        public override void Enter() => base.Enter();

        public override void LogicUpdate() => base.LogicUpdate();

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            // Vector3 direction = (_enemy.target.position - _transform.position).normalized;
            // _enemy.Rb.MovePosition(_enemy.Rb.position + direction * (Time.fixedDeltaTime * _enemy.moveSpeed));
            // Quaternion rotation = Quaternion.LookRotation(direction);
            // _transform.rotation = Quaternion.Slerp(
            //     _transform.rotation, rotation, Time.fixedDeltaTime * _enemy.rotationSpeed);
            _enemy.NavMeshAgent.SetDestination(_enemy.target.position);
        }

        public override void Exit() => base.Exit();
    }
}