using _Project.Scripts.Enemies.AI;
using _Project.Scripts.HealthSystem;
using UnityEngine;

namespace _Project.Scripts.WaveSystem
{
	public class WaveSpawner : MonoBehaviour, IWaveSpawner
	{
		private void Start()     => IsActive = true;
		private void OnEnable()  => IsActive = true;
		private void OnDisable() => IsActive = false;
		private void OnDestroy() => IsActive = false;
		public  bool IsActive    { get; set; }

		public bool Spawn(GameObject obj)
		{
			GameObject spawnedObj;
			if (!(spawnedObj = Instantiate(obj, transform.position, Quaternion.identity))) return false;
			#if UNITY_EDITOR
			totalSpawns++;
			#endif
			WaveController.Instance.totalSpawns++;
			if (spawnedObj.TryGetComponent(out Health hp)) {
				hp.onDeath.AddListener(EnemyDied);
				#if UNITY_EDITOR
				activeSpawns++;
				#endif
				WaveController.Instance.ActiveSpawns++;
			}

			if (spawnedObj.TryGetComponent(out EnemyController enemyController))
				enemyController.huntPlayerOnStart = true;

			return true;
		}


		private void EnemyDied()
		{
			#if UNITY_EDITOR
			activeSpawns--;
			#endif
			WaveController.Instance.ActiveSpawns--;
		}

		#if UNITY_EDITOR
		[SerializeField] public int totalSpawns;
		[SerializeField] public int activeSpawns;

		private void OnDrawGizmos()
		{
			if (Application.isPlaying)
				Gizmos.color = IsActive ? Color.green : Color.red;
			else
				Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, 5);
		}

		private void OnDrawGizmosSelected()
		{
			if (Application.isPlaying)
				Gizmos.color = IsActive ? Color.green : Color.red;
			else
				Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(transform.position, 5);
		}
		#endif
	}
}