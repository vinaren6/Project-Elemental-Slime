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
                _timer += Time.deltaTime;
                if (_timer >= _nextAttack)
                {
                    _hasAttacked = false;
                    if (!EnemyController.Ability.IsInRange())
                    {
                        // Debug.Log($"DEN BORTA! T_T");
                        // if (!EnemyController.Ability.StopLooping)
                            EnemyController.Ability.Stop(false);
                        _stateMachine.ChangeState(EnemyController.HuntState);
                        return;
                    }
                    else
                    {
                        // Debug.Log($"ATTACK AGAIN! :'OOO");
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

                _stateMachine.ChangeState(EnemyController.HuntState);
                return;
            }
            
            // Debug.Log($"{_transform.name} - Attack?");
            EnemyController.Animator.SetTrigger("DoAttack");
            EnemyController.Ability.Execute();

            _timer = 0f;
            _hasAttacked = true;
        }

        public override void Enter()
        {
            _nextAttack = EnemyController.Ability.GetAttackTime();
        }
    }
}