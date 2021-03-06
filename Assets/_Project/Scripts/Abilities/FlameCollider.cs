using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Enemies.AI;
using _Project.Scripts.HealthSystem;
using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public class FlameCollider : MonoBehaviour
    {
        [SerializeField][Range (5f, 10f)] private float speed;
        [SerializeField][Range (1.01f, 1.10f)] private float speedMultiplier;
        [SerializeField] private Gradient colorGradient;
        
        private Transform _transform;
        private FireAbility _ability;
        private Material _material;
        
        private float _damage;
        private float _timePercent;
        private float _aliveTime;
        private float _startSize;
        private float _targetSize;

        private Vector3 _startVelocity;
        private Vector3 _velocity;

        private void Awake()
        {
            _transform = transform;
            _material = GetComponentInChildren<MeshRenderer>().material;
        }

        public void Initialize(string newTag, FireAbility ability, float damage)
        {
            tag = newTag;
            _ability = ability;
            _damage = damage;
            _aliveTime = 0.25f;
            _timePercent = 1f;
            speedMultiplier = 1.05f;

            _startSize = 0.1f;
            _targetSize = 3f;
            
            _startVelocity = new Vector3(0f, 0f, speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(tag))
                return;
            
            if (!other.TryGetComponent(out IHealth health))
                return;
            
            // HEJ LINUS!!!!! TA BORT _timePercent OM DU VILL!
            health.ReceiveDamage(ElementalSystemTypes.Fire, _damage * _timePercent);

            if (!other.CompareTag("Enemy"))
                return;
            
            if (!other.TryGetComponent(out EnemyController enemy))
                return;

            enemy.StartDamageOverTime(ElementalSystemTypes.Fire, 5);
        }

        public void Execute(Transform spawnTransform, float newSpeed, float newSpeedMultiplier, float aliveTime)
        {
            _transform.rotation = spawnTransform.rotation;
            _transform.position = spawnTransform.position;
            _transform.SetParent(null);
            
            speedMultiplier = newSpeedMultiplier;
            _velocity = _startVelocity;
            _aliveTime = aliveTime;
            _velocity = new Vector3(0f, 0f, newSpeed);
            
            SetSize(_startSize);
            
            StartCoroutine(AliveRoutine());
        }

        private IEnumerator AliveRoutine()
        {
            float time = 0f;

            while (time < _aliveTime)
            {
                CalculateVelocity();
                _transform.Translate(_velocity * Time.deltaTime);

                SetSize(Mathf.Lerp(_startSize, _targetSize, time / _aliveTime));

                // _material.color = colorGradient.Evaluate(time / _aliveTime);
                _material.color = new Color(0f, 0f, 0f, 0f);

                // HEJ LINUS!!!!! TA BORT _timePercent OM DU VILL!
                _timePercent = Mathf.Max(0.25f, 1f - (time / _aliveTime));

                time += Time.deltaTime;

                yield return null;
            }

            _ability.ReturnToPool(this);
        }

        private void CalculateVelocity()
        {
            _velocity *= speedMultiplier;
        }

        private void SetSize(float size)
        {
            _transform.localScale = Vector3.one * size;
        }
    }
}