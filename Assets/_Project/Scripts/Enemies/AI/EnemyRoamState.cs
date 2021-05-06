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
        private bool      _isChangeDirection = false;
		private bool pause;
		
		public override void Enter()
		{
			base.Enter();
			

			ServiceLocator.Game.OnVariableChange += PauseChange;
		}

			private void PauseChange(bool pauseState)
        {
			pause = pauseState;
            if (!pause)
            {
				EnemyController.NavMeshAgent.SetDestination(_roamTargetPosition);
			}
		}
		public override void LogicUpdate()
		{
			//nedded for enemies att the start of the game otherwise they can get a position in a obsticle before it completely baked
			if (Time.time < 1)
            {
				return;
            }
			
			if (pause) {
				EnemyController.NavMeshAgent.SetDestination(_transform.position);
				return;
			}
			
			base.LogicUpdate();

			if (Vector3.Distance(_transform.position, _roamTargetPosition) < 0.75f || EnemyController.NavMeshAgent.remainingDistance > 15 || _roamTargetPosition == Vector3.zero)
			{
				_roamTargetPosition = GetNewRoamTargetPosition();
				_isChangeDirection = true;
			}

			if (_isChangeDirection )
            {
					EnemyController.NavMeshAgent.SetDestination(_roamTargetPosition);
					_isChangeDirection = false;
			}
			NavMeshPath path = new NavMeshPath();
	
			EnemyController.NavMeshAgent.CalculatePath(_roamTargetPosition, path);

		}
		
		public override void Exit() => base.Exit();

		private Vector3 GetNewRoamTargetPosition()
		{

			Vector3 randomDirection = Random.insideUnitSphere * 10;
			randomDirection += _transform.position;
			NavMeshHit hit;
			NavMesh.SamplePosition(randomDirection, out hit, 10, 1);
			Vector3 finalPosition = hit.position;

			if (NavMesh.SamplePosition(finalPosition, out hit, 1f, NavMesh.AllAreas))
            {
			
				NavMeshPath path = new NavMeshPath();
		
				EnemyController.NavMeshAgent.CalculatePath(finalPosition, path);

				if (path.status == NavMeshPathStatus.PathComplete)
				{
					return finalPosition;
				}
            }

            return GetNewRoamTargetPosition();

        }
    }
}
