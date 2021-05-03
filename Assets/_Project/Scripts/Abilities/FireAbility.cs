using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Abilities
{
	public class FireAbility : MonoBehaviour, IAbility
	{
		[SerializeField] private GameObject flamePrefab;
		[SerializeField] private Transform flamePoolTransform;
		
		[Header("Test Values")]
		[SerializeField] [Range(1f, 150f)] private float speed;
		[SerializeField] [Range(1.0f, 1.20f)] private float speedMultiplier;
		[SerializeField] [Range(0.1f, 0.5f)] private float aliveTime;

		private Transform _transform;
		private Queue<GameObject> _flamePool;
		
		private int _maxFlameColliders;
		private float _fireRate;
		private bool _canShoot;

		private float _damage;

		private void Awake()
		{
			_transform = transform;
		}

		public void Initialize(float damage)
		{
			_damage = damage;
			_maxFlameColliders = 30;
			_fireRate = 1f / _maxFlameColliders;
			_canShoot = true;
			
			if (_flamePool != null)
				return;
			
			_flamePool = new Queue<GameObject>();
			for (int i = 0; i < _maxFlameColliders; i++)
			{
				GameObject FlameObject = Instantiate(flamePrefab, flamePoolTransform);
				FlameObject.GetComponent<FlameCollider>().Initialize(this, _damage);
				FlameObject.SetActive(false);
				
				_flamePool.Enqueue(FlameObject);
			}
		}

		public void Execute()
		{
			if (!_canShoot)
				return;
			
			GameObject flameCollider = _flamePool.Dequeue();
			flameCollider.SetActive(true);
			flameCollider.GetComponent<FlameCollider>().Execute(_transform, speed, speedMultiplier, aliveTime);
			_canShoot = false;
			StartCoroutine(nameof(SpawnDelayRoutine));
		}

		public void Stop()
		{
			
		}

		private IEnumerator SpawnDelayRoutine()
		{
			yield return new WaitForSeconds(_fireRate);
			_canShoot = true;
		}

		public void ReturnToPool(GameObject flameObject)
		{
			flameObject.SetActive(false);
			flameObject.transform.SetParent(flamePoolTransform);

			_flamePool.Enqueue(flameObject);
		}
	}
}