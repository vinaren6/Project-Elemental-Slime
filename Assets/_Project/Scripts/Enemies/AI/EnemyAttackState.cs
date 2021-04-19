using _Project.Scripts.HealthSystem;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

        private float _nextAttack = 0;

        public override void Enter() => base.Enter();

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (Time.time > _nextAttack) {
                if (_enemy.target.TryGetComponent(out IHealth health)) {
                    health.ReceiveDamage(_enemy.type.Type, PlayerController.EnemyDamage);
                }
                _nextAttack = Time.time + _enemy.attackRate;
            }
        }

        public override void PhysicsUpdate() => base.PhysicsUpdate();

        public override void Exit() => base.Exit();
    }
}