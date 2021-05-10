using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.WaveSystem
{
	public class WaveController : MonoBehaviour
	{
		public static            WaveController Instance;
		[SerializeField] public  int            totalSpawns;
		[SerializeField] public  int            activeSpawns;
		[SerializeField] private GameObject[]   spawnObjects;
		[SerializeField] public  WaveSpawner[]  spawners;
		[SerializeField] private int            wave;
		[SerializeField] private float          spawnDelay;
		private                  void           Awake() => Instance = this;

		public void StartNextWave() => StartCoroutine(SpawnWave(GetWaveSpawnAmount(++wave)));

		private int GetWaveSpawnAmount(int currentWave) => (int)(10 + currentWave * 1.2f);

		private IEnumerator SpawnWave(int waveSize)
		{
			var activeWaveSpawners = spawners.Where(spawner => spawner.isActiveAndEnabled).ToList();
			for (int waveIndex = 0; waveIndex < waveSize; waveIndex++) {
				while (!activeWaveSpawners[Random.Range(0, activeWaveSpawners.Count)]
				   .Spawn(spawnObjects[Random.Range(0, spawnObjects.Length)])) ;

				yield return new WaitForSeconds(spawnDelay);
			}
		}
	}
}