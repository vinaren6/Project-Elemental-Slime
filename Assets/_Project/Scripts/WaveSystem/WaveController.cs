using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.WaveSystem
{
	public class WaveController : MonoBehaviour
	{
		public static                                 WaveController Instance;
		[SerializeField]                      public  int            totalSpawns;
		[SerializeField]                      private int            activeSpawns;
		[SerializeField]                      private GameObject[]   spawnObjects;
		[SerializeField]                      public  WaveSpawner[]  spawners;
		[SerializeField]                      private int            wave;
		[Header("Settings")] [SerializeField] private float          spawnDelay       = 0.1f;
		[SerializeField]                      private float          waveBeingDelay   = 3f;
		[SerializeField]                      private bool           autoStart        = true;
		[SerializeField]                      private bool           autoplayNextWave = true;

		#if UNITY_EDITOR
		[HideInInspector] public string debugString = "";
		#endif

		public int ActiveSpawns {
			get => activeSpawns;
			set {
				if (activeSpawns == 1 && value == 0 && autoplayNextWave)
					StartNextWave();
				#if UNITY_EDITOR
				if (activeSpawns > value)
					debugString = $"Remaining enemies: {(float) value / GetWaveSpawnAmount(wave) * 100:n0}%";
				#endif
				activeSpawns = value;
			}
		}

		private void Awake() => Instance = this;

		private void Start()
		{
			if (autoStart) StartNextWave();
		}

		public void StartNextWave() => StartCoroutine(SpawnWave(GetWaveSpawnAmount(++wave)));

		private int GetWaveSpawnAmount(int currentWave) => (int) (3 + currentWave * 1.2f);

		private IEnumerator SpawnWave(int waveSize)
		{
			var activeWaveSpawners = spawners.Where(spawner => spawner.isActiveAndEnabled).ToList();
			yield return new WaitForSeconds(waveBeingDelay);
			for (int waveIndex = 0; waveIndex < waveSize; waveIndex++) {
				#if UNITY_EDITOR
				debugString = $"Spawning: {(float) (waveIndex + 1) / waveSize * 100:n0}%";
				int fails = 0;
				#endif
				while (!activeWaveSpawners[Random.Range(0, activeWaveSpawners.Count)]
				   .Spawn(spawnObjects[Random.Range(0, spawnObjects.Length)])) {
					#if UNITY_EDITOR
					fails++;
					#endif
				}
				#if UNITY_EDITOR
				if (fails != 0)
					debugString += $"failed {fails} time{(fails == 1 ? "" : "s")}.";
				#endif
				yield return new WaitForSeconds(spawnDelay);
			}
		}
	}
}