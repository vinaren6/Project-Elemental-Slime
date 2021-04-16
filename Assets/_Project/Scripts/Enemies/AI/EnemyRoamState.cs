using UnityEngine;

namespace _Project.Scripts.Enemies.AI
{
	public class EnemyRoamState : EnemyState
	{
		public EnemyRoamState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

		private Rigidbody _rb;
		private float     _roamSpeedMultiplier = 0.5f;
		private Vector3   _roamTargetPosition;
		private Vector3   _nextRoamTargetPosition;

		public override void Enter()
		{
			base.Enter();
			_rb                     = _enemy.GetComponent<Rigidbody>();
			_roamTargetPosition     = GetNewRoamTargetPosition();
			_nextRoamTargetPosition = _roamTargetPosition;
		}

		public override void LogicUpdate()
		{
			base.LogicUpdate();
			_enemy.NavMeshAgent.SetDestination(_roamTargetPosition);
			
			if (Vector3.Distance(_transform.position, _nextRoamTargetPosition) < 1.25f) {
				_nextRoamTargetPosition = GetNewRoamTargetPosition();
			}
			if (Vector3.Distance(_transform.position, _roamTargetPosition) < 0.75f) {
				_roamTargetPosition = _nextRoamTargetPosition;
			}
		}

		public override void PhysicsUpdate() => base.PhysicsUpdate();

		public override void Exit() => base.Exit();

		private Vector3 GetNewRoamTargetPosition()
		{
			Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
			return _transform.position + randomDirection * Random.Range(5f, 5f);
		}
	}
}
