using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Abilities
{
	public class EarthAbility : MonoBehaviour, IAbility
	{
		[SerializeField] private GameObject rockPrefab;
		
		private NavMeshAgent _agent;
		private Transform _transform;
		private Queue<RockWall> _rockPool;
		
		private bool _isAttacking;
		
		private float _damage;
		private float _damageCooldownTime;
		private float _knockBackForce;

		private int _maxRockWalls;

		private void Awake()
		{
			_agent = GetComponentInParent<NavMeshAgent>();
			_transform = transform;
			SetupPool();
		}

		private void SetupPool()
		{
			if (_rockPool != null)
				return;
			
			_maxRockWalls = 32;

			_rockPool = new Queue<RockWall>();

			for (int i = 0; i < _maxRockWalls; i++)
			{
				GameObject rockObject = Instantiate(rockPrefab, _transform);
				RockWall rockWall = rockObject.GetComponent<RockWall>();
				rockWall.Initialize(this, _damage);
				rockObject.SetActive(false);
				
				_rockPool.Enqueue(rockWall);
			}	
		}
		
		public void Initialize(float damage)
		{
			_damage = damage;
			_isAttacking = false;
			_damageCooldownTime = 1f;
			_knockBackForce = 4f;
			SetupPool();
		}

		public void Execute()
		{
			if (_isAttacking)
				return;
			
			StartCoroutine(nameof(EarthQuakeRoutine));
		}

		private IEnumerator EarthQuakeRoutine()
		{
			_isAttacking = true;

			int maxRocks = 6;
			int rockCount = 0;

			float rockWidthX = 1f;
			float rockWidthZ = 1f;
			float marginX = 1f;
			float marginZ = 1f;

			Vector3 direction = _transform.forward;

			float time = 0f;
			float rockSpawnTime = 1f;

			while (rockCount < maxRocks)
			{
				rockCount++;
			}
			
			_isAttacking = false;
		}
		
		public void Stop()
		{

		}

		private IEnumerator AttackCooldownRoutine()
		{
			_isAttacking = true;
			yield return new WaitForSeconds(_damageCooldownTime);
			_isAttacking = false;
		}
	}
}