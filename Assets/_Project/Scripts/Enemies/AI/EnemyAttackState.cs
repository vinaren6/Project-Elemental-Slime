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
                _nextAttack = Time.time + _enemy.attackRate;
                Debug.Log("KILL!KILL!KILL!");
            }
        }

        public override void PhysicsUpdate() => base.PhysicsUpdate();

        public override void Exit() => base.Exit();
    }
}