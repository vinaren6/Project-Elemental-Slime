using _Project.Scripts.Managers;
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
            
            // base.LogicUpdate();
            // Vector3 targetDelta = EnemyController.Target.position - _transform.position;
            // Quaternion rot = Quaternion.LookRotation(targetDelta);
            // _transform.rotation = rot;

            if (!(Time.time > _nextAttack))
            {
                EnemyController.Ability.Stop();
                return;
            }

            RaycastHit hit;
            
            float debugHeight = 1f;
            Vector3 upOffset = Vector3.up * debugHeight;
            Debug.DrawRay(_transform.position + upOffset, _transform.TransformDirection(Vector3.forward) * 50, Color.cyan);

            if (!Physics.Raycast(_transform.position, _transform.TransformDirection(Vector3.forward), out hit, 50,
                1 << LayerMask.NameToLayer("Player")))
            {
                _stateMachine.ChangeState(EnemyController.HuntState);
                return;
            }

            if (hit.transform.tag == "Player" && hit.distance <= EnemyController.CurrentEnemyElementalStats.attackRange)
            {
                Debug.Log($"{_transform.name} - Attack?");
                EnemyController.Animator.SetTrigger("DoAttack");
                EnemyController.Ability.Execute();
                // if (EnemyController.Target.TryGetComponent(out IHealth health))
                // {
                //     health.ReceiveDamage(EnemyController.type.Type, EnemyController.AttackStrength * PlayerController.EnemyDamageMultiplier);
                // }
                _nextAttack = Time.time + EnemyController.AttackCooldownTime;
            }
            else
                _stateMachine.ChangeState(EnemyController.HuntState);
        }
    }
}