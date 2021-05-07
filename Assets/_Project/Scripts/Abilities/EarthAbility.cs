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

		private int _maxRockWalls;

		private Vector3 _rockSize;
		private Vector3 _rockMargin;

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
			
			_rockSize = new Vector3(1f, 0f, 1f);
			_rockMargin = new Vector3(1f, 0f, 1f);
		}
		
		public void Initialize(float damage)
		{
			_damage = damage;
			_isAttacking = false;
			_damageCooldownTime = 1f;
			
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
			int rocksToSpawn = 1;

			Vector3 startPosition = _transform.position;
			Vector3 direction = _transform.forward;

			float time = 0f;
			float rockSpawnTime = 0.25f;

			// Have we spawned all rocks yet?
			while (rockCount < maxRocks)
			{
				// Time to spawn more rocks???
				if (time >= rockSpawnTime)
				{
					// Spawn rock(s)
					for (int i = 1; i <= rocksToSpawn; i++)
					{
						RockWall rock = _rockPool.Dequeue();
						
						rock.Execute(GetRockPosition(startPosition, direction, rockCount, rocksToSpawn));
						
						rockCount++;
					}

					rocksToSpawn++;
					time -= rockSpawnTime;
				}

				time += Time.deltaTime;

				yield return null;
			}

			_isAttacking = false;
		}

		private Vector3 GetRockPosition(Vector3 startPosition, Vector3 direction, int rockIndex, int row)
		{
			
			
			// return new Vector3((rock-row * _rockMargin.x) * _rockSize.x, row);

			Vector3 rockPosition = Vector3.zero;

			float forward = (_rockSize.z + _rockMargin.z) * row;
			// forward = (_rockSize.z + _rockMargin.z) * (rockIndex + 1);
			float[] widths = new float[] {0f,
				(_rockSize.x + _rockMargin.x) / -2f, (_rockSize.x + _rockMargin.x) / 2f,
				(_rockSize.x + _rockMargin.x) * -1f, 0f, (_rockSize.x + _rockMargin.x)
			};

			rockPosition.x = widths[rockIndex];
			rockPosition.x = 0f;
			rockPosition.z = forward;

			rockPosition = direction * forward;
			rockPosition += Vector3.right * widths[rockIndex];

			// Vector3 newPos = new Vector3(direction.x * rockPosition.x, 0f, direction.z * rockPosition.z);

			// Debug.Log($"Rock[{rockIndex}]: pos: {startPosition} - direction: {direction} - rockPosition: {rockPosition} - newPos: {newPos} - POS: {startPosition + newPos}");
			
			// return startPosition + newPos;
			return startPosition + rockPosition;
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