using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Abilities
{
    public class WaterAbility : MonoBehaviour, IAbility
    {
        [SerializeField] private LayerMask      collisionMask;
        private                  LineRenderer   _laser;
        private                  ParticleSystem _splashEffect;
        private                  Transform      _transform;
        private                  Transform      _splashEffectTransform;
        private                  Transform      _splashEffectParent;
        private                  float          _maxDistance;
        private                  float          _radius;
        private                  bool           _splashHasStopped;
        private                  float          _damage;

        private void Awake()
        {
            GetAllComponents();
            // Initialize(0f);
        }

        private void Update()
        {
            if (Mouse.current.leftButton.isPressed)
                Execute();
            else
                _laser.SetPosition(1, _laser.GetPosition(0));
            
            if (!Mouse.current.leftButton.isPressed && _splashEffect.isPlaying && !_splashHasStopped)
            {
                // Debug.Log($"Splash Has Stopped! Soon....");
                
                Invoke(nameof(ResetSplashEffect), _splashEffect.main.duration);
                _splashHasStopped = true;
            }
        }

        private void GetAllComponents()
        {
            _laser = GetComponentInChildren<LineRenderer>();
            _splashEffect = GetComponentInChildren<ParticleSystem>();
            _transform = transform;
            _splashEffectTransform = _splashEffect.transform;
            _splashEffectParent = _splashEffectTransform.parent;
        }

        public void Initialize(float damage)
        {
            _maxDistance = 25f;
            _radius = _laser.startWidth * 0.5f;
            _splashEffect.Stop();
            _laser.SetPosition(1, _laser.GetPosition(0));
            _damage = damage;
        }

        public void Execute()
        {
            Vector3 origin = _transform.position;
            Vector3 direction = _transform.forward;

            RaycastHit hit;

            Vector3 hitPosition = _laser.GetPosition(0);
            
            if (Physics.CapsuleCast(origin, origin, _radius, direction, out hit, _maxDistance, collisionMask))
            {
                Transform hitTransform = hit.collider.transform;
                
                hitPosition.z = hit.distance;

                _splashEffectTransform.SetParent(hitTransform);
                _splashEffectTransform.localPosition = Vector3.zero;
                float rotY = _transform.eulerAngles.y - 180f;
                _splashEffectTransform.localRotation = Quaternion.Euler(0f, rotY, 0f);

                _splashHasStopped = false;

                if (hit.collider.TryGetComponent(out IHealth health))
                    health.ReceiveDamage(ElementalSystemTypes.Water, _damage);
                
                PlayEffect();
            }
            else
            {
                hitPosition.z = _maxDistance;
                StopEffect();
            }

            _laser.SetPosition(1, hitPosition);
        }

        private void ResetSplashEffect()
        {
            _splashEffect.Stop();
            _splashEffectTransform.SetParent(_splashEffectParent);
        }

        private void PlayEffect()
        {
            if (_splashEffect.isPlaying)
                return;
            
            _splashEffect.Play();
            Invoke(nameof(ResetSplashEffect), _splashEffect.main.duration);
        }

        private void StopEffect()
        {
            if (!_splashEffect.isPlaying)
                return;
            
            Invoke(nameof(ResetSplashEffect), _splashEffect.main.duration);
        }
    }
}