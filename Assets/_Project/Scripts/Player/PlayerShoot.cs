using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        private ProjectilePool _projectilePool;
        private float _nextAttack = 0;

        private void Awake()
        {
            _projectilePool = GetComponentInChildren<ProjectilePool>();
        }

        public void Fire(float attackRate, float projectileSpeed)
        {
            if (!(Time.time > _nextAttack)) return;
            
            GameObject projectile = _projectilePool.SpawnFromPool();
            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);

            _nextAttack = Time.time + attackRate;
        }
    }
}
