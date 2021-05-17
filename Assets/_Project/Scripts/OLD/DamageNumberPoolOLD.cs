using System.Collections.Generic;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class DamageNumberPoolOLD : MonoBehaviour
	{
		[SerializeField] private GameObject numberPrefab;
		[SerializeField] private int        poolSize = 100;

		private Queue<GameObject> _pool;

		private void Awake()
		{
			// ServiceLocator.ProvideDamageNumberPool(this);
		}

		private void Start()
		{
			_pool = new Queue<GameObject>();

			for (int i = 0; i < poolSize; i++) {
				GameObject number = Instantiate(numberPrefab, transform);
				_pool.Enqueue(number);
			}
		}

		public void SpawnFromPool(Vector3 position, int damage, EffectiveType colorType)
		{
			GameObject number = _pool.Dequeue();
			number.GetComponent<DamageNumberUI>().ShowDamage(position, damage, colorType);
		}

		public void ReturnNumberToPool(GameObject number)
		{
			_pool.Enqueue(number);
		}
	}
}