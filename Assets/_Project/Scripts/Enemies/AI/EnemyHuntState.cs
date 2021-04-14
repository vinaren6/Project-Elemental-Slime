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
            Vector3 direction = (_enemy.player.position - _transform.position).normalized;
            _enemy.rb.MovePosition(_enemy.rb.position + direction * (Time.fixedDeltaTime * _enemy.moveSpeed));
            Quaternion rotation = Quaternion.LookRotation(direction);
            _transform.rotation = Quaternion.Slerp(
                _transform.rotation, rotation, Time.fixedDeltaTime * _enemy.rotationSpeed);
        }

        public override void Exit() => base.Exit();
    }
}