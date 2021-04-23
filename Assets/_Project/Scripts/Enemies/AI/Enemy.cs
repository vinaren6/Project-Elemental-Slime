using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.UI;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies.AI
{
	public class Enemy : MonoBehaviour
	{
		public EnemyStateMachine StateMachine { get; set; }

		public EnemyRoamState   RoamState   { get; private set; }
		public EnemyDetectState DetectState { get; private set; }
		public EnemyHuntState   HuntState   { get; private set; }
		public EnemyAttackState AttackState { get; private set; }
		public EnemyDeathState  DeathState  { get; private set; }

		public ElementalSystemTypeCurrent type;

		public  float        moveSpeed     = 1f;
		public  float        rotationSpeed = 2.5f;
		public  float        attackRate    = 1f;
		public  Transform    target;
		public  Health       health;
		private EnemyUI		 _ui;
		private Rigidbody    _rb;
		private NavMeshAgent _navMeshAgent;
		private bool         _isBurning;

		private bool _hasDetectedPlayer;

		public EnemyUI		UI			 => _ui;
		public Rigidbody    Rb           => _rb;
		public NavMeshAgent NavMeshAgent { get => _navMeshAgent; set => _navMeshAgent = value; }
		public bool         IsBurning    { get => _isBurning;    set=> _isBurning = value; }

		private void Awake()
		{
			StateMachine = new EnemyStateMachine();
			RoamState    = new EnemyRoamState(this, StateMachine);
			DetectState  = new EnemyDetectState(this, StateMachine);
			HuntState    = new EnemyHuntState(this, StateMachine);
			AttackState  = new EnemyAttackState(this, StateMachine);
			DeathState   = new EnemyDeathState(this, StateMachine);
			StateMachine.Initialize(RoamState);

			health        = GetComponent<Health>();
			_rb           = GetComponent<Rigidbody>();
			_navMeshAgent = GetComponent<NavMeshAgent>();
			_ui			  = GetComponentInChildren<EnemyUI>();
		}

		private void Start()
		{
			// target       = GameObject.FindWithTag("Player").transform;
		}

		private void Update() => StateMachine.CurrentState.LogicUpdate();

		private void FixedUpdate() => StateMachine.CurrentState.PhysicsUpdate();

		public void CheckForPlayerDetection()
		{
			//if (other.CompareTag("Player")) { } ???
			if (_hasDetectedPlayer) return;
			_hasDetectedPlayer = true;
			target             = GameObject.FindWithTag("Player").transform;
			StateMachine.ChangeState(DetectState);
		}

		public void StartPlayerDetection() { StartCoroutine(PlayerDetection()); }

		private IEnumerator PlayerDetection()
		{
			//_rb.velocity       = Vector3.zero;
			transform.rotation = Quaternion.LookRotation((target.position - transform.position).normalized);
			yield return new WaitForSeconds(0.4f);
			float time     = 0;
			float duration = 0.1f;
			while (time < duration) {
				transform.localScale = Vector3.Lerp(
					transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), time / duration);
				time += Time.deltaTime;
				yield return null;
			}

			transform.localScale = Vector3.one;
			yield return new WaitForSeconds(0.15f);
			StateMachine.ChangeState(HuntState);
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
