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
            RaycastHit hit;
            
            Debug.DrawRay(_transform.position, _transform.TransformDirection(Vector3.forward) * EnemyController.AttackRange, Color.red);
            
            if (Physics.Raycast(_transform.position, _transform.TransformDirection(Vector3.forward), out hit, EnemyController.AttackRange, 1 << LayerMask.NameToLayer("Player")))
            {
                if (hit.transform.tag == "Player")
                {
                    EnemyController.NavMeshAgent.velocity = Vector3.zero;               
                    EnemyController.NavMeshAgent.SetDestination(_transform.position);
                    _stateMachine.ChangeState(EnemyController.AttackState);
                }
            }
        }
    }
}