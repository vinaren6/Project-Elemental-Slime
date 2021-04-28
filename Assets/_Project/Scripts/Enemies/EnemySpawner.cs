using System;
using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Enemies.AI;
using _Project.Scripts.Enemies.ScriptableObjects;
using _Project.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]                 private GameObject           enemyPrefab;
        [SerializeField]                 private ElementalSystemTypes type;
        [SerializeField]                 private int                  maxEnemiesInScene;
        [SerializeField][Range(1f, 5f)]  private float                minSpawnDelay;
        [SerializeField][Range(2f, 10f)] private float                maxSpawnDelay;
        [SerializeField]                 private Color[]              colors;
        [SerializeField]                 private int seed;
        
        private Transform _transform;
        
        public static int EnemiesInScene;
        
        private void Awake()
        {
            Random.InitState(seed);
            name = $"EnemySpawner [{type}]";
            _transform = transform;

            int index = 0;
            
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                Color color = colors[(int) type];

                if (index == 0)
                    color.a = 0.5f;
                
                meshRenderer.material.color = color;
                index++;
            }

            EnemiesInScene = 4;
        }

        private void Start()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(GetSpawnDelay());

            if (EnemiesInScene < maxEnemiesInScene) {
                EnemiesInScene++;
                Spawn();
            } else {
                StartCoroutine(SpawnRoutine());
            }
        }

        private void Spawn()
        {
            if (ServiceLocator.Game.IsPaused)
                return;
            
            GameObject enemy = Instantiate(enemyPrefab, _transform.position, Quaternion.identity, _transform);
            // enemy.GetComponent<ElementalSystemTypeCurrent>().Type = type;
            enemy.GetComponent<EnemyController>().SetupEnemyFromSpawner(GetEnemyElementalStats());
            enemy.name                                                       = $"Enemy[{type}]";

            StartCoroutine(SpawnRoutine());
        }

        private float GetSpawnDelay()
        {
            return Random.Range(minSpawnDelay, maxSpawnDelay);
        }
        
        private EnemyElementalStats GetEnemyElementalStats()
        {
            return type switch {
                ElementalSystemTypes.Earth => Resources.Load<EnemyElementalStats>("EnemyElementalStats/EarthStats"),
                ElementalSystemTypes.Wind  => Resources.Load<EnemyElementalStats>("EnemyElementalStats/WindStats"),
                ElementalSystemTypes.Water => Resources.Load<EnemyElementalStats>("EnemyElementalStats/WaterStats"),
                ElementalSystemTypes.Fire  => Resources.Load<EnemyElementalStats>("EnemyElementalStats/FireStats"),
                ElementalSystemTypes.Base  => Resources.Load<EnemyElementalStats>("EnemyElementalStats/BaseStats"),
                _                          => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}