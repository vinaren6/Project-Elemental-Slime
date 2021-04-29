using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [SerializeField] private AudioClip        shootSFX;
        private                  ProjectilePool   _projectilePool;
        private                  PlayerController _player;
        private                  float            _nextAttack = 0;
        
        private void Awake()
        {
            _projectilePool = GetComponentInChildren<ProjectilePool>();
            _player         = GetComponent<PlayerController>();
        }

        public void Fire()
        {
            if (Time.time < _nextAttack)
                return;
            
            GameObject projectile = _projectilePool.SpawnFromPool();
            projectile.GetComponent<Rigidbody>().AddForce(transform.forward * _player.ProjectileSpeed);
            _nextAttack = Time.time + _player.AttackCooldownTime;
            ServiceLocator.Audio.PlaySFX(shootSFX);
            _player.HasAttacked = true;
        }
    }
}
