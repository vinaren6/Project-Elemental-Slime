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

            base.LogicUpdate();
            EnemyController.NavMeshAgent.SetDestination(EnemyController.Target.position);

            if (!EnemyController.Ability.IsInRange())
                return;
            
            EnemyController.NavMeshAgent.velocity = Vector3.zero;               
            EnemyController.NavMeshAgent.SetDestination(_transform.position);
            _stateMachine.ChangeState(EnemyController.AttackState);
        }
    }
}