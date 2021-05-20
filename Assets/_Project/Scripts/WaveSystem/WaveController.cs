using System.Collections;
using System.Linq;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.WaveSystem
{
	public class WaveController : MonoBehaviour
	{
		public static WaveController Instance;

		[SerializeField]                      public  UnityEvent    onWaveStart;
		[SerializeField]                      public  int           totalSpawns;
		[SerializeField]                      private int           activeSpawns;
		[SerializeField]                      private GameObject[]  spawnObjects;
		[SerializeField]                      public  WaveSpawner[] spawners;
		[SerializeField]                      private Health        player;
		[SerializeField]                      public  int           wave;
		[Header("Settings")] [SerializeField] private int           maxActiveSpawnsAtSameTime = 15;
		[SerializeField]                      private float         spawnDelay                = 0.2f;
		[SerializeField]                      private float         waveBeingDelay            = 3f;
		[SerializeField]                      private bool          autoStart                 = true;
		[SerializeField]                      private bool          autoplayNextWave          = true;
		[SerializeField]                      private float         healAmountOnNewRound      = 20;

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

		public void StartNextWave()
		{
			if (player != null && healAmountOnNewRound > 0)
				player.ReceiveHealth(ElementalSystemTypes.Base, healAmountOnNewRound);
			StartCoroutine(SpawnWave(GetWaveSpawnAmount(++wave)));
			onWaveStart.Invoke();
		}

		private int GetWaveSpawnAmount(int currentWave) => (int) (3 + currentWave * 1.2f);

		private IEnumerator SpawnWave(int waveSize)
		{
			var activeWaveSpawners = spawners.Where(spawner => spawner.isActiveAndEnabled).ToList();
			#if UNITY_EDITOR
			int fails = 0;
			debugString = "Spawning: 0%";
			#endif
			yield return new WaitForSeconds(waveBeingDelay);
			for (int waveIndex = 0; waveIndex < waveSize; waveIndex++) {
				#if UNITY_EDITOR
				debugString = $"Spawning: {(float) (waveIndex + 1) / waveSize * 100:n0}%";
				#endif
				while (!activeWaveSpawners[Random.Range(0, activeWaveSpawners.Count)]
				   .Spawn(spawnObjects[Random.Range(0, spawnObjects.Length)])) {
					#if UNITY_EDITOR
					fails++;
					#endif
				}
				#if UNITY_EDITOR
				if (fails != 0)
					debugString += $" -failed {fails} time{(fails == 1 ? "" : "s")}.";
				#endif
				yield return new WaitForSeconds(spawnDelay);
				while (activeSpawns >= maxActiveSpawnsAtSameTime) yield return new WaitForSeconds(1);
			}
		}
	}
}