using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class ProjectilePool : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int poolSize = 10;

        private Queue<GameObject> _projectilePool;
        private float _projectileLifespan = 4f;

        private void Start()
        {
            _projectilePool = new Queue<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab);
                projectile.SetActive(false);
                _projectilePool.Enqueue(projectile);
            }
        }

        public GameObject SpawnFromPool()
        {
            GameObject projectile = _projectilePool.Dequeue();
            projectile.SetActive(true);
            StartCoroutine(ReturnProjectileToPool(projectile));
            return projectile;
        }

        private IEnumerator ReturnProjectileToPool(GameObject projectile)
        {
            yield return new WaitForSeconds(_projectileLifespan);
            projectile.SetActive(false);
            _projectilePool.Enqueue(projectile);
        }
    }
}