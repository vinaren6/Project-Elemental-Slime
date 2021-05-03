using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Abilities
{
	public class EarthAbility : MonoBehaviour, IAbility
	{
		private NavMeshAgent               _agent;
		private Transform                  _transform;
		private BoxCollider                _attackTrigger;
		private ParticleSystem             _earthEffect;
		private ParticleSystem.ShapeModule _effectShape;
		private Vector3                    _startPosition;
		private Vector3                    _startSize;
		private Transform                  _playerParent;
		private float                      _damage;
		private bool                       _canDealDamage;
		private float                      _damageCooldownTime;
		private Vector3                    _attackDirection;
		private float                      _knockBackStrength;


		private void Awake()
		{
			_agent         = GetComponentInParent<NavMeshAgent>();
			_transform     = gameObject.transform;
			_attackTrigger = GetComponent<BoxCollider>();
			_earthEffect   = GetComponentInChildren<ParticleSystem>();
			_effectShape   = _earthEffect.shape;
			_startPosition = _attackTrigger.transform.localPosition;
			_startSize     = _attackTrigger.size;
			_playerParent  = transform.parent;
		}

		private void OnDisable() => ResetEarthQuakeModifications();

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IHealth health)) {
				float earthMultiplier = Mathf.Clamp(_attackTrigger.size.x, 5f, 10f) / 10;
				health.ReceiveDamage(ElementalSystemTypes.Earth, earthMultiplier * _damage);
			}
			if (other.TryGetComponent(out NavMeshAgent agent)) {
				StartCoroutine(HitPushBack(agent, _attackDirection, _knockBackStrength, 1f));
			}
		}

		public void Initialize(float damage)
		{
			_damage                = damage;
			_canDealDamage         = true;
			_damageCooldownTime    = 1f;
			_attackTrigger.enabled = false;
			_knockBackStrength   = 4f;
		}
		
		public void Execute()
		{
			if (!_canDealDamage)
				return;
			
			_canDealDamage = false;
			StartCoroutine(EarthQuake());
			// StartCoroutine(AttackCooldownRoutine());
		}

		private IEnumerator EarthQuake()
		{
			_agent.velocity        = Vector3.zero;
			_attackTrigger.enabled = true;
			transform.SetParent(null);
			_attackDirection = _transform.forward;
			
			PlayEffect();
			
			float   time              = 0;
			float   duration          = 1f;
			
			while (time < duration) {
				float smoothStepLerp = time / duration;
				smoothStepLerp      =  smoothStepLerp * smoothStepLerp * (3f - 2f * smoothStepLerp);
				_transform.position += _attackDirection * Mathf.Lerp(0.2f, 0, smoothStepLerp);
				_attackTrigger.size = new Vector3((Mathf.Lerp(2f, 20f, smoothStepLerp)), _startSize.y, _startSize.z);
				_effectShape.scale  = new Vector3((Mathf.Lerp(2.5f, 22f, smoothStepLerp)), 1, 1);

				time += Time.deltaTime;
				yield return null;
			}
			yield return new WaitForSeconds(0.25f);
			_earthEffect.Stop();
			yield return new WaitForSeconds(0.25f);
			_canDealDamage = true;
			ResetEarthQuakeModifications();
		}
		
		public void Stop()
		{

		}
		
		private IEnumerator AttackCooldownRoutine()
		{
			_canDealDamage = false;
			yield return new WaitForSeconds(_damageCooldownTime);
			_canDealDamage = true;
		}
		
		private void ResetEarthQuakeModifications()
		{
			_earthEffect.Stop();
			transform.SetParent(_playerParent);
			_attackTrigger.enabled       = false;
			_transform.localPosition     = _startPosition;
			_transform.localEulerAngles  = Vector3.zero;
			_attackTrigger.size          = _startSize;
			_effectShape.scale           = new Vector3(2, 1, 1);
		}
		
		private void PlayEffect()
		{
			if (_earthEffect.isPlaying)
				return;
			_earthEffect.Play();
		}
		
		private IEnumerator HitPushBack(NavMeshAgent agent, Vector3 direction, float pushStrength, float rotationFreezeTime)
		{
			if (agent == null)
				yield break;
			
			agent.velocity       = direction * pushStrength;
			agent.updateRotation = false;
			yield return new WaitForSeconds(rotationFreezeTime);
			agent.updateRotation = true;
		}
	}
}