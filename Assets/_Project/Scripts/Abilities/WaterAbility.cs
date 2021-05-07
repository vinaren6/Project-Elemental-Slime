using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public class WaterAbility : MonoBehaviour, IAbility
    {
        #region Variables
        
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] [Range(15f, 30f)] private float maxDistance;
        [SerializeField] [Range(0.1f, 2f)] private float radius;
        
        private                  Transform      _transform;
        private                  Transform      _splashEffectTransform;
        private                  Transform      _splashEffectParent;
        private                  LineRenderer   _laser;
        private                  ParticleSystem _splashEffect;
        private                  bool           _splashHasStopped;
        private                  bool           _canDealDamage;
        private                  float          _damage;
        private                  float          _damageCooldownTime;

        #endregion

        #region Start Methods

        private void Reset()
        {
            maxDistance = 25f;
            radius = 0.5f;
        }

        private void Awake()
        {
            GetAllComponents();
        }
        
        private void GetAllComponents()
        {
            _transform             = transform;
            _laser                 = GetComponentInChildren<LineRenderer>();
            _splashEffect          = GetComponentInChildren<ParticleSystem>();
            _splashEffectTransform = _splashEffect.transform;
            _splashEffectParent    = _splashEffectTransform.parent;
        }

        public void Initialize(float damage)
        {
            _laser.startWidth = radius * 2f;
            _laser.SetPosition(1, _laser.GetPosition(0));
            _splashEffect.Stop();
            _damage             = damage;
            _damageCooldownTime = 0.2f;
            _canDealDamage      = true;
        }
        
        #endregion

        #region Methods
        
        public void Execute()
        {
            RaycastHit hit = new RaycastHit();

            Vector3 hitPosition = _laser.GetPosition(0);

            if (LaserDidHit(ref hit))
            {
                ApplyHitLogic(hit);
                
                hitPosition.z = hit.distance + radius;
                
                PlayEffect();
            }
            else
            {
                hitPosition.z = maxDistance;
                StopEffect();
            }

            _laser.SetPosition(1, hitPosition);
        }

        private bool LaserDidHit(ref RaycastHit hit)
        {
            Vector3 origin = _transform.position;
            Vector3 direction = _transform.forward;
            
            bool didHit = Physics.CapsuleCast(origin, origin, radius, direction, out hit, maxDistance, collisionMask);
            // return Physics.CapsuleCast(origin, origin, radius, direction, out hit, maxDistance, collisionMask);
            
            if (didHit)
                Debug.DrawRay(origin, direction * hit.distance, Color.red);
            else
                Debug.DrawRay(origin, direction * maxDistance, Color.yellow);
            
            // Debug.Log($"hit.distance: {hit.distance}");

            return didHit;
        }

        private void ApplyHitLogic(RaycastHit hit)
        {
            Transform hitTransform = hit.collider.transform;

            _splashEffectTransform.SetParent(hitTransform);
            _splashEffectTransform.localPosition = Vector3.zero;
            float rotY = _transform.eulerAngles.y - 180f;
            _splashEffectTransform.localRotation = Quaternion.Euler(0f, rotY, 0f);

            _splashHasStopped = false;

            if (!_canDealDamage)
                return;

            if (!hit.collider.TryGetComponent(out IHealth health))
                return;
            
            health.ReceiveDamage(ElementalSystemTypes.Water, _damage);
            StartCoroutine(AttackCooldownRoutine());
        }

        private IEnumerator AttackCooldownRoutine()
        {
            _canDealDamage = false;
            yield return new WaitForSeconds(_damageCooldownTime);
            _canDealDamage = true;
        }

        public void Stop()
        {
            _laser.SetPosition(1, _laser.GetPosition(0));
            
            if (_splashHasStopped)
                return;

            Invoke(nameof(ResetSplashEffect), _splashEffect.main.duration);
            _splashHasStopped = true;
        }
        
        #endregion

        #region Effect Methods

        private void PlayEffect()
        {
            if (_splashEffect.isPlaying)
                return;
            
            _splashEffect.Play();
        }

        private void StopEffect()
        {
            if (!_splashEffect.isPlaying)
                return;
            
            Invoke(nameof(ResetSplashEffect), _splashEffect.main.duration);
        }
        
        private void ResetSplashEffect()
        {
            _splashEffect.Stop();
            _splashEffectTransform.SetParent(_splashEffectParent);
        }
        
        #endregion
    }
}