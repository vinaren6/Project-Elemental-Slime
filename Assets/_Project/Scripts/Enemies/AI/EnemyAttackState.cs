using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

        private float _nextAttack = 0;
        
        public override void LogicUpdate()
        {
            if (ServiceLocator.Game.IsPaused)
                return;
            
            base.LogicUpdate();
            if (Time.time > _nextAttack) {
                if (_enemy.target.TryGetComponent(out IHealth health)) {
                    health.ReceiveDamage(_enemy.type.Type, _enemy.attackStrength * PlayerController.EnemyDamageMultiplier);
                }
                _nextAttack = Time.time + _enemy.attackCooldownTime;
            }
        }
    }
}