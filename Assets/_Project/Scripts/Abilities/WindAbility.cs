using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Abilities
{
	public class WindAbility : MonoBehaviour, IAbility
	{
		#region Variables
		
		[SerializeField] private Collider      selfDamageCollider;
		[SerializeField] private AnimationCurve velocityCurve;
		
		private NavMeshAgent _agent;
		private BoxCollider _attackTrigger;
		private float _damage;
		// private float _attackCooldownTime;
		private float _rechargeTime;
		private bool _canDealDamage;
		private bool _canAttack;
		private int _maxCharges;
		private int _currentCharges;
		
		#endregion

		#region Start Methods

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			if (selfDamageCollider == null)
				Debug.LogWarning($"No Collider is set for the \"{name}\" \"{nameof(selfDamageCollider)}\"");
			
			_agent = GetComponentInParent<NavMeshAgent>();
			_attackTrigger = GetComponent<BoxCollider>();
		}

		public void Initialize(string newTag, float damage, Collider selfCollider = null)
		{
			tag = newTag;
			_attackTrigger.enabled = false;
			_damage                = damage;
			// _attackCooldownTime    = 0.1f;
			_rechargeTime          = 1f;
			_maxCharges            = 3;
			_currentCharges        = _maxCharges;
			_canAttack = true;
			
			if (selfCollider != null)
				selfDamageCollider = selfCollider;
		}
		
		#endregion
		
		private void OnDisable() => ResetDashingModifications();

		private void OnTriggerEnter(Collider other)
		{
			if (!_canDealDamage)
				return;

			if (other.CompareTag(tag))
				return;

			if (!other.TryGetComponent(out IHealth health))
				return;
			
			health.ReceiveDamage(ElementalSystemTypes.Wind, _damage);
			
			if (!other.TryGetComponent(out NavMeshAgent agent))
				return;

			agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		}

		#region Methods

		public void Execute()
		{
			if (!CanBeExecuted())
				return;

			_canAttack = false;
			
			StartCoroutine(WindDashRoutine());
		}

		private bool CanBeExecuted()
		{
			return _currentCharges > 0 && _canAttack;
		}

		private IEnumerator WindDashRoutine()
		{
			if (_currentCharges >= _maxCharges)
				StartCoroutine(RechargeRoutine());
			else
				_currentCharges--;

			ActivateAttackTrigger();

			float time = 0f;
			float attackTime = 0.1f;
			float duration = 0.4f;

			// float maxDistance = 10f;
			
			// Debug.Log($"DASH! START");

			Vector3 direction = transform.forward;
			float speed = 120f;
			
			// Debug.Log($"_agent: {_agent.name}");
			
			while (time <= duration)
			{
				_canDealDamage = time < attackTime;
				_attackTrigger.enabled = time < attackTime;

				float newSpeed = velocityCurve.Evaluate(time / duration) * speed;

				// Debug.Log($"Dash.speed: {newSpeed} - time: {time}");
				
				_agent.Move(direction * (newSpeed * Time.deltaTime));

				time += Time.deltaTime;
				
				yield return null;
			}
			
			// _agent.velocity = Vector3.zero;
			
			DeactivateAttackTrigger();
			
			// Debug.Log($"DASH! End...");

			// _canDealDamage             = false;
			// selfDamageCollider.enabled = false;
			// _agent.velocity            = Vector3.zero;
			//
			// float time = 0;
			// float duration = 0.05f;
			//
			// while (time < duration)
			// {
			// 	float smoothStepLerp = time / duration;
			// 	smoothStepLerp = smoothStepLerp * smoothStepLerp * (3f - 2f * smoothStepLerp);
			// 	_agent.Move(transform.forward * Mathf.Lerp(5f, 0f, smoothStepLerp));
			// 	time           += Time.deltaTime;
			// 	yield return null;
			// }
			//
			// _attackTrigger.enabled = true;
			// yield return new WaitForSeconds(_attackCooldownTime);
			// ResetDashingModifications();
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
			// ResetDashingModifications();
		}

		private void ResetDashingModifications()
		{
			_attackTrigger.enabled = false;
			selfDamageCollider.enabled = true;
			_canDealDamage = true;
		}

		private void ActivateAttackTrigger()
		{
			_attackTrigger.enabled = true;
			selfDamageCollider.enabled = false;
			_agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
		}

		private void DeactivateAttackTrigger()
		{
			_attackTrigger.enabled = false;
			selfDamageCollider.enabled = true;
			_agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
		}
		
		#endregion
	}
}