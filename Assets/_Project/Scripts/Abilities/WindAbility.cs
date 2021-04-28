using System;
using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Abilities
{
	public class WindAbility : MonoBehaviour, IAbility
	{
		private PlayerController _player;
		private NavMeshAgent     _agent;
		private BoxCollider      _playerCollider;
		private BoxCollider      _attackTrigger;
		private float            _nextAttack = 0;

		private void Awake()
		{
			_player         = GetComponentInParent<PlayerController>();
			_agent          = GetComponentInParent<NavMeshAgent>();
			_playerCollider = _player.gameObject.GetComponent<BoxCollider>();
			_attackTrigger  = GetComponent<BoxCollider>();
			//Application.targetFrameRate = 300;
		}

		private void Update() 
		{ 
			if (Mouse.current.leftButton.isPressed)
				Execute(); 
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IHealth health))
				health.ReceiveDamage(ElementalSystemTypes.Wind, PlayerController.PlayerDamage);
		}

		public void Execute()
		{
			if (!(Time.time > _nextAttack))
				return;

			_playerCollider.enabled = false;
			_agent.velocity         = Vector3.zero;
			Time.timeScale          = 0.5f;
			_player.IsDashing       = true;
			StartCoroutine(WindDash());

			_nextAttack = Time.time + _player.AttackCooldownTime;
		}

		private IEnumerator WindDash()
		{
			float   time          = 0;
			float   duration      = 0.03f;
			while (time < duration) {
				_agent.Move(transform.forward * Mathf.Lerp(5f, 0f, time / duration));
				time        += Time.deltaTime;
				yield return null;
			}
			_attackTrigger.enabled = true;
			
			yield return new WaitForSeconds(0.1f);
			
			_attackTrigger.enabled  = false;
			_playerCollider.enabled = true;
			_player.IsDashing       = false;
			Time.timeScale          = 1f;
		}
	}
}