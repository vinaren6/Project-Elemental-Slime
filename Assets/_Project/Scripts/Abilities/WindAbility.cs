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
		[SerializeField] private GameObject effectObject;
		
		private NavMeshAgent _agent;
		private BoxCollider _attackTrigger;
		private Transform _transform;
		private float _damage;
		private float _maxDistance;
		private float _rechargeTime;
		private float _attackDuration;
		private float _attackCooldownTime;
		private bool _canDealDamage;
		private bool _canAttack;
		private int _maxCharges;
		private int _currentCharges;
		
		public bool StopLooping => false;

		#endregion

		#region Start Methods

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			// if (selfDamageCollider == null)
			// 	Debug.LogWarning($"No Collider is set for the \"{name}\" \"{nameof(selfDamageCollider)}\"");
			
			_agent = GetComponentInParent<NavMeshAgent>();
			_attackTrigger = GetComponent<BoxCollider>();
			_transform = transform;
			effectObject.SetActive(false);
		}

		public void Initialize(string newTag, float damage, Collider selfCollider = null)
		{
			tag = newTag;
			_attackTrigger.enabled = false;
			_damage                = damage;
			_attackDuration = 0.4f;
			_attackCooldownTime = 0.5f;
			_maxDistance = 8f;
			_rechargeTime          = 1f;
			_maxCharges            = 3;
			_currentCharges        = _maxCharges;
			_canAttack = true;
			
			if (selfCollider != null)
				selfDamageCollider = selfCollider;
		}

		public void Initialize(string newTag, float damage, float distance, Collider selfCollider = null)
		{
			Initialize(newTag, damage, selfCollider);
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
			float attackTime = 0.2f;

			Vector3 direction = transform.forward;
			float speed = 120f;
			
			// Debug.Log($"atkTime: {_attackDuration} + 0.25 = {_attackDuration + 0.25f}");

			while (time <= _attackDuration)
			{
				_canDealDamage = time < attackTime;
				_attackTrigger.enabled = time < attackTime;

				float newSpeed = velocityCurve.Evaluate(time / _attackDuration) * speed;

				_agent.Move(direction * (newSpeed * Time.deltaTime));

				time += Time.deltaTime;
				
				yield return null;
			}

			if (CompareTag("Enemy"))
			{
				time = 0f;
				
				while (time < 0.25f)
				{
					yield return null;
					
					time += Time.deltaTime;
				}
				
				// Debug.Log($"FREE!!");
			}

			DeactivateAttackTrigger();
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
		
		public void Stop(bool isPlayer = true)
		{
			if (isPlayer)
				_canAttack = true;
		}

		public bool IsInRange()
		{
			RaycastHit hit = new RaycastHit();

			Vector3 center = _transform.position + _attackTrigger.center;
			Vector3 halfExtents = _attackTrigger.size * 0.5f;
			Vector3 direction = _transform.forward;
			Quaternion orientation = _transform.rotation;
			
			float debugHeight = 1f;
            Vector3 upOffset = Vector3.up * debugHeight;
            Debug.DrawRay(_transform.position + upOffset,
	            _transform.TransformDirection(Vector3.forward) * _maxDistance,
	            Color.red);
            
			// Debug.Log($"center: {center} - ext: {halfExtents} - dir: {direction} - ori: {orientation}");
			
			// TODO Calc some math to know how far the enemy will move and how big its attack trigger is for better results.
			
			if (!Physics.BoxCast(center,
				halfExtents,
				direction,
				out hit,
				orientation,
				_maxDistance,
				1 << LayerMask.NameToLayer("Player")))
				return false;

			// Debug.Log($"hit.name: {hit.collider.name} is in RANGE!!!");
			
			return true;
		}

		public bool CanAttack()
		{
			return _canAttack;
		}

		public float GetAttackTime()
		{
			// Debug.Log($"Get Attack TiME: {_attackDuration + _attackCooldownTime}");
			return _attackDuration + _attackCooldownTime;
		}

		private void ResetDashingModifications()
		{
			_attackTrigger.enabled = false;
			if (selfDamageCollider != null)
				selfDamageCollider.enabled = true;
			_canDealDamage = true;
		}

		private void ActivateAttackTrigger()
		{
			_attackTrigger.enabled = true;
			
			if (CompareTag("Player"))
				selfDamageCollider.enabled = false;

			_agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
			effectObject.SetActive(true);
		}

		private void DeactivateAttackTrigger()
		{
			_attackTrigger.enabled = false;
			
			if (CompareTag("Player"))
				selfDamageCollider.enabled = true;
			
			_agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
			_canAttack = true;
			effectObject.SetActive(false);
		}
		
		#endregion
	}
}