using _Project.Scripts.Managers;
using UnityEngine;

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
            
            if (!EnemyController.Target.gameObject.activeInHierarchy)
                return;
            
            base.LogicUpdate();

            Vector3 targetPosition = EnemyController.Target.position;
            EnemyController.NavMeshAgent.SetDestination(targetPosition);
            float distanceToTarget = Vector3.Distance(targetPosition, _transform.position);

            if (distanceToTarget > 2f && !EnemyController.Ability.IsInWalkRange())
                return;
            
            EnemyController.NavMeshAgent.velocity = Vector3.zero;               
            EnemyController.NavMeshAgent.SetDestination(_transform.position);
            _stateMachine.ChangeState(EnemyController.AttackState);
        }
    }
}