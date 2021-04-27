using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;
namespace _Project.Scripts.Enemies.AI
{
	public class EnemyRoamState : EnemyState
	{
		public EnemyRoamState(EnemyController enemyController, EnemyStateMachine stateMachine) : base(enemyController, stateMachine) { }
		
		// private float     _roamSpeedMultiplier = 0.5f;
		private Vector3   _roamTargetPosition;
		private Vector3   _nextRoamTargetPosition;
        private bool      _isChangeDirection = true;
		private bool pause;
		
		public override void Enter()
		{
			base.Enter();
			
			_roamTargetPosition     = GetNewRoamTargetPosition();
			_nextRoamTargetPosition = _roamTargetPosition;
			ServiceLocator.Game.OnVariableChange += PauseChange;
		}
		private void PauseChange(bool pauseState)
        {
			Debug.Log("test");
			pause = pauseState;
            if (!pause)
            {
				EnemyController.NavMeshAgent.SetDestination(_roamTargetPosition);
			}
		}
		public override void LogicUpdate()
		{
			if (pause) {
				EnemyController.NavMeshAgent.SetDestination(_transform.position);
				return;
			}
			
			base.LogicUpdate();
			
			if (_isChangeDirection )
            {
				NavMeshPath path = new NavMeshPath();
				EnemyController.NavMeshAgent.CalculatePath(_roamTargetPosition, path);
				
                if (path.status == NavMeshPathStatus.PathComplete)
                {
					EnemyController.NavMeshAgent.SetDestination(_roamTargetPosition);
					_isChangeDirection = false;
                }
                else
                {
					_roamTargetPosition = GetNewRoamTargetPosition();
				}
				
			}

			
			if (Vector3.Distance(_transform.position, _nextRoamTargetPosition) < 1.25f) {
				
			}
			if (Vector3.Distance(_transform.position, _roamTargetPosition) < 0.75f || EnemyController.NavMeshAgent.remainingDistance > 15) {
				_roamTargetPosition = GetNewRoamTargetPosition();
				_isChangeDirection = true;
			}
		}
		
		public override void Exit() => base.Exit();

		private Vector3 GetNewRoamTargetPosition()
		{
			Vector3 randomDirection  = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
			float   randomWalkRadius = Random.Range(10f, 10f);
			Vector3 NewTargetPosition = _transform.position + randomDirection * randomWalkRadius;
			NewTargetPosition.y = 0;
			//Vector3 terrainPosition = Terrain.activeTerrain.GetPosition(rand);
			
			NavMeshHit hit;
			if (NavMesh.SamplePosition(NewTargetPosition, out hit, 1f, NavMesh.AllAreas))
			{
				return NewTargetPosition;
			}
			
			return GetNewRoamTargetPosition();
			
			// if (NavMesh.SamplePosition(NewTargetPosition, out NavMeshHit hit, randomWalkRadius, 1)) {
			// 	return hit.position;
			// }
			// Debug.Log("Enemy has to get new RoamTargetPosition!");
			// return GetNewRoamTargetPosition();
		}
	}
}
