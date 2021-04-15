using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerShootProjectiles : MonoBehaviour
    {
        private ProjectilePool _projectilePool;
        private float _nextAttack = 0;

        private void Awake()
        {
            _projectilePool = GetComponent<ProjectilePool>();
        }

        public void Fire(Transform player, float attackRate, float projectileSpeed)
        {
            if (Time.time > _nextAttack)
            {
                GameObject projectile = _projectilePool.SpawnFromPool();
                projectile.transform.position = player.position;
                projectile.GetComponent<Rigidbody>().AddForce(player.forward * projectileSpeed);

                _nextAttack = Time.time + attackRate;
            }
        }
    }
}
