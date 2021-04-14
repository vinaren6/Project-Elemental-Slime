using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Enemies.AI
{
	public class EnemyAI : MonoBehaviour
	{
		private enum State
		{
			Roam,
			Detect,
			Hunt,
			Attack,
			Death
		}
		
		[Header("ENEMY SETTING:")]
		[SerializeField] private float roamSpeed = 0.2f;
		[SerializeField] private float huntSpeed     = 1.0f;
		[SerializeField] private float rotationSpeed = 2.5f;
		[SerializeField] private float attackRate    = 1.0f;
		
		private Rigidbody _rb;
		private Vector3   _roamTargetPosition;
		private Vector3   _nextRoamTargetPosition;
		private bool      _hasDetectedPlayer;
		private State     _activeState;
		private Transform _target;
		private float     _nextAttack = 0;

		private void Awake() => _activeState = State.Roam;

		private void Start()
		{
			_rb                     = GetComponent<Rigidbody>();
			_roamTargetPosition     = GetNewRoamTargetPosition();
			_nextRoamTargetPosition = _roamTargetPosition;
			_target                 = GameObject.FindWithTag("Player").transform;
		}

		private void Update()
		{
			switch (_activeState) {
				case State.Roam:
					transform.position = Vector3.Lerp(transform.position, _roamTargetPosition, Time.deltaTime * roamSpeed);
					transform.rotation = Quaternion.Slerp(
						transform.rotation, Quaternion.LookRotation((_nextRoamTargetPosition - transform.position).normalized), Time.deltaTime * rotationSpeed);
					if (Vector3.Distance(transform.position, _nextRoamTargetPosition) < 1.25f) {
						_nextRoamTargetPosition = GetNewRoamTargetPosition();
					}
					if (Vector3.Distance(transform.position, _roamTargetPosition) < 0.75f) {
						_roamTargetPosition = _nextRoamTargetPosition;
					}
					break;
				case State.Detect:
					break;
				case State.Hunt:
					break;
				case State.Attack:
					if (Time.time >_nextAttack) {
						_nextAttack = Time.time + attackRate;
						print("KILL!KILL!KILL!");
					}
					break;
				case State.Death:
					//Play DeathStuff and destroy
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void FixedUpdate()
		{
			switch (_activeState) {
				case State.Roam:
					break;
				case State.Detect:
					break;
				case State.Hunt:
					if (!_target) return;
					Vector3 direction = (_target.position - transform.position).normalized;
					_rb.MovePosition(_rb.position + direction * (Time.fixedDeltaTime * huntSpeed));
					Quaternion rotation = Quaternion.LookRotation(direction);
					transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
					break;
				case State.Attack:
					break;
				case State.Death:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		
		private Vector3 GetNewRoamTargetPosition()
		{
			Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
			return transform.position + randomDirection * Random.Range(6f, 12f);
		}

		public void CheckForPlayerDetection()
		{
			//if (other.CompareTag("Player")) { } ???
			if (_hasDetectedPlayer) return;
			_activeState      = State.Detect;
			_hasDetectedPlayer = true;
			StartCoroutine(PlayerDetection());
		}
		
		private IEnumerator PlayerDetection()
		{
			_rb.velocity       = Vector3.zero;
			transform.rotation = Quaternion.LookRotation((_target.position - transform.position).normalized);
			yield return new WaitForSeconds(0.4f);
			float   time       = 0;
			float   duration   = 0.1f;
			while (time < duration) {
				transform.localScale =  Vector3.Lerp(transform.localScale, new Vector3(1.5f, 1.5f, 1.5f), time / duration);
				time                 += Time.deltaTime;
				yield return null;
			}
			transform.localScale = Vector3.one;
			yield return new WaitForSeconds(0.15f);
			_activeState = State.Hunt;
		}

		private void OnCollisionEnter(Collision other)
		{
			if (other.collider.CompareTag("Player")) {
				_activeState = State.Attack;
			}
		}

		private void OnCollisionExit(Collision other)
		{
			if (other.collider.CompareTag("Player")) {
				_activeState = State.Hunt;
			}
		}
	}
}