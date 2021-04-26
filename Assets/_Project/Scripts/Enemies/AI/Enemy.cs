using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Enemies.ScriptableObjects;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _Project.Scripts.Enemies.AI
{
	public class Enemy : MonoBehaviour
	{
		[Header("ACTIVE SETTINGS/STATS:")] 
		[SerializeField] private EnemySettings baseSettings;
		[SerializeField] private EnemyElementalStats currentEnemyElementalStats;
		
		public ElementalSystemTypeCurrent type;
		
		public EnemyStateMachine StateMachine { get; set; }

		public EnemyRoamState   RoamState   { get; private set; }
		public EnemyDetectState DetectState { get; private set; }
		public EnemyHuntState   HuntState   { get; private set; }
		public EnemyAttackState AttackState { get; private set; }
		public EnemyDeathState  DeathState  { get; private set; }
		
		public float rotationSpeed = 2.5f;
		public float attackStrength;
		public float attackCooldownTime;
		
		private Transform    _target;
		private Health       _health;
		private EnemyUI      _ui;
		private NavMeshAgent _navMeshAgent;

		private bool         _isBurning;
		[HideInInspector] public float damageOverTimeTotalTicks;

		private bool _hasDetectedPlayer;

		public Transform    Target       => _target;
		public Health       Health       => _health;
		public EnemyUI      UI           => _ui;
		public NavMeshAgent NavMeshAgent => _navMeshAgent;

		public bool IsBurning {
			get => _isBurning;
			set => _isBurning = value;
		}

		private void Awake()
		{
			StateMachine = new EnemyStateMachine();
			RoamState    = new EnemyRoamState(this, StateMachine);
			DetectState  = new EnemyDetectState(this, StateMachine);
			HuntState    = new EnemyHuntState(this, StateMachine);
			AttackState  = new EnemyAttackState(this, StateMachine);
			DeathState   = new EnemyDeathState(this, StateMachine);
			StateMachine.Initialize(RoamState);

			_health       = GetComponent<Health>();
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_ui           = GetComponentInChildren<EnemyUI>();

			type.Type            = currentEnemyElementalStats.elementType;
			_health.MaxHitPoints = currentEnemyElementalStats.maxHitPoints;
			_navMeshAgent.speed  = baseSettings.moveSpeed          * currentEnemyElementalStats.moveSpeedMultiplier;
			attackStrength       = baseSettings.attackStrength     * currentEnemyElementalStats.attackStrengthMultiplier;
			attackCooldownTime   = baseSettings.attackCooldownTime * currentEnemyElementalStats.attackRateMultiplier;
		}

		private void Update() => StateMachine.CurrentState.LogicUpdate();

		public void CheckForPlayerDetection()
		{
			//if (other.CompareTag("Player")) { } ???
			if (_hasDetectedPlayer)
				return;
			_hasDetectedPlayer = true;
			_target = GameObject.FindWithTag("Player").transform;
			StateMachine.ChangeState(DetectState);
		}

		public void StartPlayerDetection()
		{
			StartCoroutine(PlayerDetection());
		}

		private IEnumerator PlayerDetection()
		{
			transform.rotation = Quaternion.LookRotation((_target.position - transform.position).normalized);
			yield return new WaitForSeconds(0.4f);
			float time = 0;
			float duration = 0.1f;
			while (time < duration)
			{
				transform.localScale = Vector3.Lerp(
					transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), time / duration);
				time += Time.deltaTime;
				yield return null;
			}

			transform.localScale = Vector3.one;
			yield return new WaitForSeconds(0.15f);
			StateMachine.ChangeState(HuntState);
		}

		public void StartDamageOverTime(ElementalSystemTypes damageType, int totalTicks)
		{
			if (damageType == type.Type)
				return;

			if (damageType != ElementalSystemTypes.Fire)
				return;

			if (type.Type == ElementalSystemTypes.Water)
				return;
			
			_isBurning = true;
			damageOverTimeTotalTicks = totalTicks;
		}

		private void OnCollisionEnter(Collision other)
		{
			if (other.collider.CompareTag("Player"))
				StateMachine.ChangeState(AttackState);
		}

		private void OnCollisionExit(Collision other)
		{
			if (other.collider.CompareTag("Player"))
				StateMachine.ChangeState(HuntState);
		}
	}
}
