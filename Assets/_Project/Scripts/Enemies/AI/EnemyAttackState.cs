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
            Vector3 targetDelta = EnemyController.Target.position - _transform.position;
            Quaternion rot = Quaternion.LookRotation(targetDelta);
            _transform.rotation = rot;

            if (Time.time > _nextAttack)
            {
                RaycastHit hit;
                if (Physics.Raycast(_transform.position, _transform.TransformDirection(Vector3.forward), out hit, 50, ~9))
                {
                    if (hit.transform.tag == "Player" && hit.distance <= EnemyController.AttackRange)
                    {
                        if (EnemyController.Target.TryGetComponent(out IHealth health))
                        {
                            health.ReceiveDamage(EnemyController.type.Type, EnemyController.AttackStrength * PlayerController.EnemyDamageMultiplier);
                        }
                        _nextAttack = Time.time + EnemyController.AttackCooldownTime;
                    }
                    else
                        _stateMachine.ChangeState(EnemyController.HuntState);
                }
            }
        }
    }
}