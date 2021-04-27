using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private AudioClip        shootSFX;
        private                  ProjectilePool   _projectilePool;
        private                  float            _nextAttack = 0;
        private                  PlayerController _player;
        
        private void Awake()
        {
            _projectilePool = GetComponentInChildren<ProjectilePool>();
            _player         = GetComponent<PlayerController>();
        }

        public void Fire(float attackCooldownTime, float projectileSpeed)
        {
            if (!(Time.time > _nextAttack))
                return;
            
            GameObject projectile = _projectilePool.SpawnFromPool();
            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
            _nextAttack = Time.time + attackCooldownTime;
            ServiceLocator.Audio.PlaySFX(shootSFX);
            _player.HasAttacked = true;
        }
    }
}
