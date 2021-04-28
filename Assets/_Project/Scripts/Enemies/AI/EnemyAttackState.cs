using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(EnemyController enemyController, EnemyStateMachine stateMachine) : base(enemyController, stateMachine) { }

        private float _nextAttack = 0;
        
        public override void LogicUpdate()
        {
            if (ServiceLocator.Game.IsPaused)
                return;
            
            base.LogicUpdate();
            if (Time.time > _nextAttack) {
                if (EnemyController.Target.TryGetComponent(out IHealth health)) {
                    health.ReceiveDamage(EnemyController.type.Type, EnemyController.AttackStrength * PlayerController.EnemyDamageMultiplier);
                }
                _nextAttack = Time.time + EnemyController.AttackCooldownTime;
            }
        }
    }
}