using System;
using System.Collections;
using _Project.Scripts.Abilities;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Enemies.ScriptableObjects;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace _Project.Scripts.Enemies.AI
{
	public class EnemyController : MonoBehaviour
	{
		[Header("ACTIVE SETTINGS/STATS:")] 
		[SerializeField] private EnemySettings baseSettings;
		[SerializeField] private EnemyElementalStats currentEnemyElementalStats;
		[SerializeField] private int _attackRange;
		public ElementalSystemTypeCurrent type;
		
		public EnemyStateMachine StateMachine { get; set; }
		public EnemyRoamState    RoamState    { get; private set; }
		public EnemyDetectState  DetectState  { get; private set; }
		public EnemyHuntState    HuntState    { get; private set; }
		public EnemyAttackState  AttackState  { get; private set; }
		public EnemyDeathState   DeathState   { get; private set; }
		
		private Transform    _target;
		private Health       _health;
		private EnemyUI      _ui;
		private NavMeshAgent _navMeshAgent;
		private IAbility _ability;
		
		private float _attackStrength;
		private float _attackCooldownTime;
		private int   _damageOverTimeTotalTicks;

		private bool  _isBurning;
		private bool  _hasDetectedPlayer = false;

		public Transform    Target                   => _target;
		public Health       Health                   => _health;
		public EnemyUI      UI                       => _ui;
		public NavMeshAgent NavMeshAgent             => _navMeshAgent;
		public IAbility Ability => _ability;
		public float        AttackStrength           => _attackStrength;
		public float        AttackCooldownTime       => _attackCooldownTime;
		public int          DamageOverTimeTotalTicks => _damageOverTimeTotalTicks;
		public int          AttackRange              => _attackRange;

		public bool IsBurning { get => _isBurning; set => _isBurning = value; }

		private void Awake()
		{
			GetComponentReferences();
			SetStats();
			InstantiateMesh();
		}

		private void Start()
		{
			InitializeStateMachine();
			_target = GameObject.FindWithTag("Player").transform;
		}

		private void Update() => StateMachine.CurrentState.LogicUpdate();

		public void CheckForPlayerDetection()
		{
			if (_hasDetectedPlayer)
				return;
			_hasDetectedPlayer = true;
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
			_damageOverTimeTotalTicks = totalTicks;
		}

		public void ChangeToDeathState() => StateMachine.ChangeState(DeathState);

		private void OnCollisionEnter(Collision other)
		{
			//if (other.collider.CompareTag("Player"))
				//StateMachine.ChangeState(AttackState);
		}

		private void OnCollisionExit(Collision other)
		{
			//if (other.collider.CompareTag("Player"))
			//	StateMachine.ChangeState(HuntState);
		}
		
		private void InitializeStateMachine()
		{
			StateMachine = new EnemyStateMachine();
			RoamState    = new EnemyRoamState(this, StateMachine);
			DetectState  = new EnemyDetectState(this, StateMachine);
			HuntState    = new EnemyHuntState(this, StateMachine);
			AttackState  = new EnemyAttackState(this, StateMachine);
			DeathState   = new EnemyDeathState(this, StateMachine);
			StateMachine.Initialize(RoamState);
		}
		
		private void GetComponentReferences()
		{
			_health       = GetComponent<Health>();
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_ui           = GetComponentInChildren<EnemyUI>();
			Instantiate(currentEnemyElementalStats.ability, transform);
			_ability = GetComponentInChildren<IAbility>();
			_ability.Initialize(tag, 1f, GetComponent<Collider>());
			
			// print($"_ability.name: {_ability.gameObject.name}");
		}
		
		private void SetStats()
		{
			type.Type            = currentEnemyElementalStats.elementType;
			_health.MaxHitPoints = currentEnemyElementalStats.maxHitPoints;
			_navMeshAgent.speed  = baseSettings.moveSpeed          * currentEnemyElementalStats.moveSpeedMultiplier;
			_attackStrength      = baseSettings.attackStrength     * currentEnemyElementalStats.attackStrengthMultiplier;
			_attackCooldownTime  = baseSettings.attackCooldownTime * currentEnemyElementalStats.attackRateMultiplier;
		}

		private void InstantiateMesh()
		{ 
			GameObject EnemyMesh = Instantiate(currentEnemyElementalStats.mesh, transform.position, Quaternion.identity);
			EnemyMesh.transform.SetParent(transform.Find("Graphics"));
		}

		public void SetupEnemyFromSpawner(EnemyElementalStats elementalStats)
		{
			currentEnemyElementalStats = elementalStats;
			SetStats();
			transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
			InstantiateMesh();
		}
		
		public void SetStatsFromEditorWindow() => SetStats();
	}
}
