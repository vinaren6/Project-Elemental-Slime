using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Abilities
{
	public class WindAbility : MonoBehaviour, IAbility
	{
		[SerializeField] private Collider      selfDamageCollider;
		
		private NavMeshAgent     _agent;
		private BoxCollider      _attackTrigger;
		private float            _damage;
		private float            _attackCooldownTime;
		private float            _rechargeTime;
		private bool             _canDealDamage;
		private bool _canAttack;
		private int              _maxCharges;
		private int              _currentCharges;

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			if (selfDamageCollider == null)
				Debug.LogWarning($"No Collider is set for the \"{name}\" \"{nameof(selfDamageCollider)}\"");
			
			_agent              = GetComponentInParent<NavMeshAgent>();
			_attackTrigger      = GetComponent<BoxCollider>();
		}

		public void Initialize(float damage)
		{
			_attackTrigger.enabled = false;
			_damage                = damage;
			_attackCooldownTime    = 0.1f;
			_rechargeTime          = 1f;
			_maxCharges            = 3;
			_currentCharges        = _maxCharges;
		}
		
		private void OnDisable() => ResetDashingModifications();

		private void OnTriggerEnter(Collider other)
		{
			if (other.CompareTag(other.tag))
			{
				Debug.Log($"myTag: {tag} - other.tag = {other.tag}");
				return;
			}
			
			Debug.Log($"myTag: {tag} - other.tag = {other.tag}");

			if (other.TryGetComponent(out IHealth health)){
				health.ReceiveDamage(ElementalSystemTypes.Wind, _damage);
			}
		}

		public void Execute()
		{
			if (!CanBeExecuted())
				return;

			_canAttack = false;
			
			StartCoroutine(WindDashRoutine());
		}

		private bool CanBeExecuted()
		{
			if (_currentCharges > 0 && _canDealDamage && _canAttack)
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
			selfDamageCollider.enabled = false;
			_agent.velocity             = Vector3.zero;

			float   time          = 0;
			float   duration      = 0.06f;
			
			while (time < duration)
			{
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
			while (_currentCharges < _maxCharges)
			{
				yield return new WaitForSeconds(_rechargeTime);
				_currentCharges++;
			}
		}
		
		public void Stop()
		{
			_canAttack = true;
			ResetDashingModifications();
		}

		private void ResetDashingModifications()
		{
			_attackTrigger.enabled = false;
			selfDamageCollider.enabled = true;
			_canDealDamage = true;
		} 
	}
}