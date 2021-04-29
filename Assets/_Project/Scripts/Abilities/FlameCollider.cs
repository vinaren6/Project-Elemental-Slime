using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public class FlameCollider : MonoBehaviour
    {
        private float _damage;

        public void Initialize(float damage)
        {
            _damage = damage;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IHealth health))
            {
                health.ReceiveDamage(ElementalSystemTypes.Fire, _damage);
            }
        }
    }
}