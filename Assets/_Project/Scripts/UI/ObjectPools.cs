using System.Collections.Generic;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class ObjectPools : MonoBehaviour
	{
		[System.Serializable]
		public class Pool
		{
			public PoolType   poolType;
			public GameObject prefab;
			public int        size;
			public Transform  parent;
		}

		public List<Pool>                              pools;
		public Dictionary<PoolType, Queue<GameObject>> PoolDictionary;
		
		private void Awake()
		{
		ServiceLocator.ProvideObjectPools(this);
		}

		private void Start()
		{
			PoolDictionary = new Dictionary<PoolType, Queue<GameObject>>();

			foreach (Pool pool in pools) {
				Queue<GameObject> objectPool = new Queue<GameObject>();

				for (int i = 0; i < pool.size; i++) {
					GameObject obj = Instantiate(pool.prefab, pool.parent);
					objectPool.Enqueue(obj);
				}
				
				PoolDictionary.Add(pool.poolType, objectPool);
			}
		}

		public void SpawnFromPool(PoolType type, Vector3 position, int damage, EffectiveType colorType)
		{
			if (type != PoolType.DamageNumber) {
				Debug.Log("Wrong PoolType in this function!");
				return;
			}
			
			GameObject objectToSpawn = PoolDictionary[type].Dequeue();
			objectToSpawn.GetComponent<DamageNumberUI>().ShowDamage(position, damage, colorType);
		}

		public void SpawnFromPool(PoolType type, Vector3 position)
		{
			if (type == PoolType.DamageNumber) {
				Debug.Log("Wrong PoolType in this function!");
				return;
			}
			
			GameObject objectToSpawn = PoolDictionary[type].Dequeue();
			if(type == PoolType.KillText) {
				objectToSpawn.GetComponent<KillTextUI>().ShowKillText(position);
			}
			else {
				objectToSpawn.GetComponent<ComboTextUI>().ShowComboText(position);
			}
		}

		public void ReturnObjectToPool(PoolType type, GameObject obj)
		{
			PoolDictionary[type].Enqueue(obj);
		}
	}
}