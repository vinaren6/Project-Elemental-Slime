using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerShootProjectiles : MonoBehaviour
    {
        [SerializeField] private GameObject projectile;

        private float _nextAttack = 0;
        
        public void Fire(float attackRate)
        {
            if (Time.time > _nextAttack)
            {
                print("FIRE!");
                _nextAttack = Time.time + attackRate;
            }
        }
    }
}
