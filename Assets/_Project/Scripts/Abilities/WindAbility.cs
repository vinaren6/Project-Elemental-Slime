using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Abilities
{
	public class WindAbility : MonoBehaviour, IAbility
	{
		private NavMeshAgent     _agent;
		private BoxCollider      _selfDamageCollider;
		private BoxCollider      _attackTrigger;
		private float            _damage;
		private float            _attackCooldownTime;
		private float            _RechargeTime;
		private bool             _canDealDamage;
		private int              _maxCharges;
		private int              _currentCharges;

		private void Awake()
		{
			_agent              = GetComponentInParent<NavMeshAgent>();
			_selfDamageCollider = GetComponentInParent<BoxCollider>();
			_attackTrigger      = GetComponent<BoxCollider>();
		}

		public void Initialize(float damage)
		{
			_attackTrigger.enabled = false;
			_damage                = damage;
			_attackCooldownTime    = 0.1f;
			_RechargeTime          = 1f;
			_maxCharges            = 3;
			_currentCharges        = _maxCharges;
		}
		
		private void OnDisable() => ResetDashingModifications();

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IHealth health)){
				health.ReceiveDamage(ElementalSystemTypes.Wind, _damage);
			}
		}

		public void Execute()
		{
			if (!CanBeExecuted())
				return;
			
			StartCoroutine(WindDashRoutine());
		}

		private bool CanBeExecuted()
		{
			if (_currentCharges > 0 && _canDealDamage)
				return true;

			return false;
		}

		private IEnumerator WindDashRoutine()
		{
			if (_currentCharges >= _maxCharges)
				StartCoroutine(RechargeRoutine());
			else
				_currentCharges--;
			
			_canDealDamage              = false;
			_selfDamageCollider.enabled = false;
			_agent.velocity             = Vector3.zero;

			float   time          = 0;
			float   duration      = 0.06f;
			while (time < duration) {
				float smoothStepLerp = time / duration;
				smoothStepLerp = smoothStepLerp * smoothStepLerp * (3f - 2f * smoothStepLerp);
				_agent.Move(transform.forward * Mathf.Lerp(5f, 0f, smoothStepLerp));
				time           += Time.deltaTime;
				yield return null;
			}
			_attackTrigger.enabled = true;
			yield return new WaitForSeconds(_attackCooldownTime);
			ResetDashingModifications();
		}
		
		private IEnumerator RechargeRoutine()
		{
			_currentCharges--;
			while (_currentCharges < _maxCharges) {
				yield return new WaitForSeconds(_RechargeTime);
				_currentCharges++;
				print(_currentCharges);
			}
		}
		
		public void Stop()
		{
			ResetDashingModifications();
		}

		private void ResetDashingModifications()
		{
			_attackTrigger.enabled      = false;
			_selfDamageCollider.enabled = true;
			_canDealDamage              = true;
		} 
	}
}