using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
    public class EnemyAttackState : EnemyState
    {
        public EnemyAttackState(EnemyController enemyController, EnemyStateMachine stateMachine) : base(enemyController, stateMachine) { }

        private float _timer;
        private float _nextAttack = 1f;
        private bool _hasAttacked;

        public override void LogicUpdate()
        {
            if (ServiceLocator.Game.IsPaused)
                return;

            if (_hasAttacked)
            {
                EnemyController.NavMeshAgent.destination = _transform.position;
                // Debug.Log($"TIME: {_timer} - NEXTaTTACK: {_nextAttack}");
                _timer += Time.deltaTime;
                if (_timer >= _nextAttack)
                {
                    // EnemyController.NavMeshAgent.destination = _transform.position;
                    // EnemyController.NavMeshAgent.SetDestination(_transform.position);
                    _hasAttacked = false;
                    if (!EnemyController.Ability.IsInRange())
                    {
                        // Debug.Log($"DEN BORTA! T_T time: {_timer}");
                        // if (!EnemyController.Ability.StopLooping)
                        EnemyController.Ability.Stop(false);
                        EnemyController.Animator.SetBool("IsAttacking", false);
                        _stateMachine.ChangeState(EnemyController.HuntState);
                        return;
                    }
                    else
                    {
                        // Debug.Log($"ATTACK AGAIN! :'OOO TIME: {_timer}");
                    }
                }
                return;
            }

            if (!EnemyController.Ability.CanAttack())
            {
                // Debug.Log($"Can't attack... T_T");
                if (!EnemyController.Ability.StopLooping)
                {
                    EnemyController.Ability.Stop(false);
                    // Debug.Log($"KAN INTE ATTACKERA! T_T");
                }

                EnemyController.Animator.SetBool("IsAttacking", false);
                _stateMachine.ChangeState(EnemyController.HuntState);
                return;
            }
            
            // Debug.Log($"{_transform.name} - Attack?");
            EnemyController.Animator.SetBool("IsAttacking", true);
            EnemyController.Animator.SetTrigger("DoAttack");
            EnemyController.Ability.Execute();

            _timer = 0f;
            _hasAttacked = true;
        }

        public override void Enter()
        {
            _nextAttack = EnemyController.Ability.GetAttackTime();
        }

        public override void Exit()
        {
            base.Exit();
            EnemyController.Animator.SetBool("IsAttacking", false);
        }
    }
}