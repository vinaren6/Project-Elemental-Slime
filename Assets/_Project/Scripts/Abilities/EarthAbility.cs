using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Abilities
{
	public class EarthAbility : MonoBehaviour, IAbility
	{
		[SerializeField] private GameObject rockPrefab;
		[SerializeField] private Transform poolParent;
		[SerializeField] private Transform rockTransformParent;
		[SerializeField] private BoxCollider rangeCollider;
		[SerializeField] private AudioClip earthSFX;
		
		private NavMeshAgent _agent;
		private Transform _transform;
		private Queue<RockWall> _rockPool;

		private Transform[] _rockTransforms;
		
		private bool _isAttacking;
		private bool _canAttack;
		
		private float _damage;
		private float _damageCooldownTime;
		private float _rockSpawnTime;

		private int _maxRockWalls;

		private Vector3 _rockSize;
		private Vector3 _rockMargin;
		
		private List<IHealth> _damagedEnemies;

		public bool StopLooping => false;

		private void Awake()
		{
			_agent = GetComponentInParent<NavMeshAgent>();
			_transform = transform;
			_rockTransforms = new Transform[rockTransformParent.childCount];
			for (int i = 0; i < _rockTransforms.Length; i++)
			{
				_rockTransforms[i] = rockTransformParent.GetChild(i);
			}

			// SetupPool();
		}

		private void SetupPool()
		{
			if (_rockPool != null)
				return;
			
			_maxRockWalls = 32;

			_rockPool = new Queue<RockWall>();

			for (int i = 0; i < _maxRockWalls; i++)
			{
				GameObject rockObject = Instantiate(rockPrefab, poolParent);
				RockWall rockWall = rockObject.GetComponent<RockWall>();
				rockWall.Initialize(this, tag);
				rockObject.SetActive(false);
				
				_rockPool.Enqueue(rockWall);
			}
			
			_rockSize = new Vector3(1f, 0f, 1f);
			_rockMargin = new Vector3(1f, 0f, 1f);
		}
		
		public void Initialize(string newTag, float damage, Collider selfCollider = null)
		{
			tag = newTag;
			_damage = damage;
			_isAttacking = false;
			_canAttack = true;
			_damageCooldownTime = 1f;
			_rockSpawnTime = 0.1f;
			
			SetupPool();
		}

		public void Initialize(string newTag, float damage, float distance, Collider selfCollider = null)
		{
			Initialize(newTag, damage, selfCollider);
		}

		public bool DidExecute()
		{
			if (_isAttacking || !_canAttack)
				return false;
			
			_damagedEnemies = new List<IHealth>();
			
			StartCoroutine(nameof(EarthQuakeRoutine));
			return true;
		}

		private IEnumerator EarthQuakeRoutine()
		{
			_isAttacking = true;
			
			// Debug.Log($"BEGIN ROCKS! {Time.timeSinceLevelLoad}");

			if (CompareTag("Player"))
				_canAttack = false;

			int maxRocks = 6;
			int rockCount = 0;
			int rocksToSpawn = 1;

			Vector3 startPosition = _transform.position;
			Vector3 direction = _transform.forward;

			Vector3[] rockPositions = new Vector3[_rockTransforms.Length];
			for (int i = 0; i < rockPositions.Length; i++)
			{
				rockPositions[i] = _rockTransforms[i].position;
			}

			float time = 0f;
			float duration = 0.5f;

			if (CompareTag("Enemy"))
			{
				time = -0.325f + _rockSpawnTime;
				duration = 1.41f;
			}

			// Have we spawned all rocks yet?
			while (rockCount < maxRocks)
			{
				if (rocksToSpawn <= 2 || CompareTag("Enemy"))
				{
					_agent.velocity = Vector3.zero;
				}

				// Time to spawn more rocks???	
				if (time >= _rockSpawnTime * rocksToSpawn && rocksToSpawn <= 3)
				{
					// Spawn rock(s)
					ServiceLocator.Audio.PlaySFX(earthSFX, 1.4f);
					for (int i = 1; i <= rocksToSpawn; i++)
					{
						RockWall rock = _rockPool.Dequeue();
						
						rock.Execute(rockPositions[rockCount], GetDamage(rocksToSpawn), ref _damagedEnemies);
						
						// rock.Execute(GetRockPosition(startPosition, direction, rockCount, rocksToSpawn));
						
						rockCount++;
					}

					rocksToSpawn++;
					time -= _rockSpawnTime;
				}

				time += Time.deltaTime;

				yield return null;
			}

			while (time < duration)
			{
				time += Time.deltaTime;

				yield return null;
			}
			
			// Debug.Log($".................END.....................? {Time.timeSinceLevelLoad}");

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

		public void Stop(bool isPlayer = true)
		{
			if (isPlayer)
				_canAttack = true;
		}

		public bool IsInRange()
		{
			return Physics.CheckBox(rangeCollider.transform.position + rangeCollider.center,
				rangeCollider.size * 0.5f,
				_transform.rotation,
				1 << LayerMask.NameToLayer("Player"));
		}

		public bool IsInWalkRange()
		{
			return Physics.CheckBox(rangeCollider.transform.position + rangeCollider.center,
				rangeCollider.size * 0.5f,
				_transform.rotation,
				1 << LayerMask.NameToLayer("Player"));
		}

		public bool CanAttack()
		{
			return !_isAttacking;
		}

		public float GetAttackTime()
		{
			return _rockSpawnTime * 3 * 2;
		}

		private IEnumerator AttackCooldownRoutine()
		{
			_isAttacking = true;
			yield return new WaitForSeconds(_damageCooldownTime);
			_isAttacking = false;
		}

		private float GetDamage(int row)
		{
			float multiplier = row switch
			{
				2 => 1.25f,
				3 => 1.5f,
				_ => 1f
			};

			return _damage * multiplier;
		}
		
		public void ReturnToPool(RockWall rock)
		{
			rock.transform.SetParent(poolParent);
			rock.gameObject.SetActive(false);

			_rockPool.Enqueue(rock);
		}
	}
}