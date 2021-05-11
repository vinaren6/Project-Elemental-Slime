using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class KillFeedbackPool : MonoBehaviour
	{
		[SerializeField] private GameObject killTextPrefab;
		[SerializeField] private int        poolSize = 100;

		private Queue<GameObject> _pool;

		private void Start()
		{
			_pool = new Queue<GameObject>();

			for (int i = 0; i < poolSize; i++) {
				GameObject killText = Instantiate(killTextPrefab, transform);
				_pool.Enqueue(killText);
			}
		}

		public void SpawnFromPool(Vector3 position)
		{
			GameObject killText = _pool.Dequeue();
			killText.GetComponent<KillTextUI>().ShowKillText(position);
		}

		public void ReturnNumberToPool(GameObject number)
		{
			_pool.Enqueue(number);
		}
	}
}