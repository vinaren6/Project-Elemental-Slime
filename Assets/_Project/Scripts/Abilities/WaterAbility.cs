using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Player;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts.Abilities
{
    public class WaterAbility : MonoBehaviour, IAbility
    {
        #region Variables
        
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private Transform waterRayTransform;
        [SerializeField] [Range(15f, 30f)] private float maxDistance;
        [SerializeField] [Range(0.1f, 2f)] private float radius;
        [SerializeField] private AudioClip waterSFX;
        
        private                  Transform      _transform;
        private SplashEffect _splashEffect;
        private AudioSource _audioSource;

        private                  bool           _canDealDamage;
        private                  float          _damage;
        private                  float          _damageCooldownTime;

        private bool _stopLooping;
        
        public bool StopLooping => _stopLooping;

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
            _transform = transform;
            _splashEffect = GetComponentInChildren<SplashEffect>();
            _audioSource = gameObject.AddComponent<AudioSource>();
        }

        public void Initialize(string newTag, float damage, Collider selfCollider = null)
        {
            tag = newTag;
            _stopLooping = true;
            SetWaterRayScale(0f);
            _splashEffect.StopEffect();
            _damage             = damage;
            _damageCooldownTime = 0.2f;
            _canDealDamage      = true;
            _audioSource.outputAudioMixerGroup = Resources.Load<AudioMixer>("AudioMixer").FindMatchingGroups("SFX")[0];
            _audioSource.loop   = true;
            _audioSource.volume = 1.7f;
            _audioSource.clip   = waterSFX;

            collisionMask = CompareTag("Player")
                ? 1 << LayerMask.NameToLayer("Enemy")
                : 1 << LayerMask.NameToLayer("Player");
        }

        public void Initialize(string newTag, float damage, float distance, Collider selfCollider = null)
        {
            Initialize(newTag, damage, selfCollider);
            maxDistance = distance;
        }
        
        #endregion

        #region Methods
        
        public bool DidExecute()
        {
            RaycastHit hit = new RaycastHit();
            Vector3 origin = _transform.position;
            Vector3 direction = GameObject.FindObjectOfType<PlayerController>().transform.position - transform.position;
            bool didHit = Physics.CapsuleCast(origin, origin, radius, transform.parent.gameObject.transform.forward, out hit, maxDistance, collisionMask);

            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
            
            if (didHit)
            {
                ApplyHitLogic(hit);
                
                SetWaterRayScale(hit.distance);
                
                _splashEffect.StartEffect(hit.transform);
            }
            else
                SetWaterRayScale(maxDistance);

            return false;
        }

        private bool LaserDidHit(ref RaycastHit hit, float distance)
        {
            Vector3 origin = _transform.position;
            //Vector3 direction = _transform.forward;
            Vector3 direction = GameObject.FindObjectOfType<PlayerController>().transform.position - transform.position;
            bool didHit = Physics.CapsuleCast(origin, origin, radius, direction, out hit, distance, collisionMask);
            // return Physics.CapsuleCast(origin, origin, radius, direction, out hit, maxDistance, collisionMask);
            
            if (didHit)
                Debug.DrawRay(origin, direction * hit.distance, Color.red);
            else
                Debug.DrawRay(origin, direction * maxDistance, Color.yellow);

            return didHit;
        }

        private void ApplyHitLogic(RaycastHit hit)
        {
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

        public void Stop(bool isPlayer = true)
        {
            SetWaterRayScale(0f);
            _splashEffect.StopEffect();
            _audioSource.Stop();
        }

        public bool IsInRange()
        {
            var turnSpeed = 5.0f;
            var _dir = GameObject.FindObjectOfType<PlayerController>().transform.position - transform.parent.gameObject.transform.position;
            _dir.Normalize();
            transform.parent.gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_dir), turnSpeed * Time.deltaTime);
            RaycastHit hit = new RaycastHit();
            return LaserDidHit(ref hit, maxDistance);
            
            // TODO Better calc for checking range.
            // Transform p = GameObject.FindObjectOfType<PlayerController>().transform;
            //
            // if (Vector3.Distance(_transform.position, p.position) < maxDistance)
            //     return true;
            // return false;
        }
        public bool IsInWalkRange()
        {
            RaycastHit hit = new RaycastHit();
            return LaserDidHit(ref hit, maxDistance - 5);

            // TODO Better calc for checking range.
            // Transform p = GameObject.FindObjectOfType<PlayerController>().transform;
            //
            // if (Vector3.Distance(_transform.position, p.position) < maxDistance - 5)
            //     return true;
            // return false;
        }

        public bool CanAttack()
        {
            return true;
        }

        public float GetAttackTime()
        {
            return 0f;
        }
        
        private void SetWaterRayScale(float amount)
        {
            waterRayTransform.localScale = new Vector3(1f, 1f, amount);
        }

        #endregion
    }
}