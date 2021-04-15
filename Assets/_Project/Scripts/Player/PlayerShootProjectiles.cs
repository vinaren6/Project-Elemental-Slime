using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerShootProjectiles : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;

        private float _nextAttack = 0;
        
        public void Fire(Transform player, float attackRate, float projectileSpeed)
        {
            if (Time.time > _nextAttack)
            {
                GameObject projectile = Instantiate(projectilePrefab, player.position, Quaternion.identity);

                projectile.GetComponent<Rigidbody>().AddForce(player.forward * projectileSpeed);
                
                print("FIRE!");
                _nextAttack = Time.time + attackRate;
            }
        }
    }
}
