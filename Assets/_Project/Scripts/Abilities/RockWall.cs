using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public class RockWall : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _animationCurve;
        
        private Transform _transform;
        private EarthAbility _ability;
        private Material _material;
        private Color _originalColor;
        private Collider _collider;

        private List<IHealth> _damagedEnemies;

        private float _damage;

        private void Awake()
        {
            _material = GetComponentInChildren<MeshRenderer>().material;
            _originalColor = _material.color;
            _collider = GetComponent<Collider>();
        }

        public void Initialize(EarthAbility ability, string newTag)
        {
            _transform = transform;
            _ability = ability;
            tag = newTag;
        }

        public void Execute(Vector3 spawnPosition, float damage)
        {
            _damage = damage;

            _damagedEnemies = new List<IHealth>();
            
            _transform.SetParent(null);
            _transform.position = spawnPosition;
            _material.color = _originalColor;
            gameObject.SetActive(true);

            StartCoroutine(nameof(RockRoutine));
        }

        private IEnumerator RockRoutine()
        {
            float time = 0f;

            float height = 1.5f;

            float upDuration = 0.25f;
            float aliveTime = 0.5f;

            Vector3 localScale = _transform.localScale;
            localScale.y = 0f;
            _transform.localScale = localScale;
            Vector3 targetScale = localScale;
            targetScale.y = height;

            _collider.enabled = true;

            while (time < aliveTime)
            {
                _collider.enabled = time < upDuration;

                targetScale.y = height * _animationCurve.Evaluate(time / aliveTime);
                _transform.localScale = targetScale;
                
                // _transform.localScale = Vector3.Lerp(_transform.localScale, targetScale,
                //     _animationCurve.Evaluate(time / aliveTime));
                
                // Debug.Log($"ev.value: {_animationCurve.Evaluate(time / aliveTime)}");
                
                // _transform.localScale = Vector3.Lerp(_transform.localScale, targetScale, time / upDuration);
                
                time += Time.deltaTime;
                yield return null;
            }

            _collider.enabled = false;

            // time = 0f;
            // while (time < aliveTime)
            // {
            //     Color noAlphaColor = _material.color;
            //     noAlphaColor.a = Mathf.Lerp(1f, 0f, time / aliveTime);
            //     _material.color = noAlphaColor;
            //     
            //     time += Time.deltaTime;
            //     yield return null;
            // }

            _ability.ReturnToPool(this);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(tag))
                return;
            
            // Debug.Log($"col.name: {other.name}");

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
