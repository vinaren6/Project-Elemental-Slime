using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public class RockWall : MonoBehaviour
    {
        private Transform _transform;
        private EarthAbility _ability;

        private List<IHealth> _damagedEnemies;

        private float _damage;

        public void Initialize(EarthAbility ability)
        {
            _transform = transform;
            _ability = ability;
        }

        public void Execute(Vector3 spawnPosition, float damage)
        {
            _damage = damage;

            _damagedEnemies = new List<IHealth>();
            
            _transform.SetParent(null);
            _transform.position = spawnPosition;
            gameObject.SetActive(true);

            StartCoroutine(nameof(RockRoutine));
        }

        private IEnumerator RockRoutine()
        {
            float time = 0f;

            float height = 4f;

            float upDuration = 0.4f;
            float aliveTime = 0.5f;

            _transform.localScale = new Vector3(1f, 0f, 1f);
            Vector3 targetScale = Vector3.one;
            targetScale.y = height;

            while (time < upDuration)
            {
                _transform.localScale = Vector3.Lerp(_transform.localScale, targetScale, time / upDuration);

                time += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(aliveTime);
            
            _ability.ReturnToPool(this);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(tag))
                return;

            if (other.TryGetComponent(out IHealth health))
            {
                if (_damagedEnemies.Contains(health))
                    return;
                
                health.ReceiveDamage(ElementalSystemTypes.Earth, _damage);
                
                _damagedEnemies.Add(health);
        	}
        	// if (other.TryGetComponent(out NavMeshAgent agent))
         //    {
        	// 	StartCoroutine(HitPushBack(agent, _direction, _knockBackForce, 1f));
        	// }
        }
        
        // private IEnumerator HitPushBack(NavMeshAgent agent, Vector3 direction, float pushStrength, float rotationFreezeTime)
        // {
        //     if (agent == null)
        //         yield break;
			     //
        //     agent.velocity       = direction * pushStrength;
        //     agent.updateRotation = false;
        //     yield return new WaitForSeconds(rotationFreezeTime);
        //     agent.updateRotation = true;
        // }
    }
}
