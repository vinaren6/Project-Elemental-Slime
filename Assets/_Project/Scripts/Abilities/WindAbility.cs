using System;
using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
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
		private float            _damage;
		private float            _nextAttack = 0;

		private void Awake()
		{
			_player         = GetComponentInParent<PlayerController>();
			_agent          = GetComponentInParent<NavMeshAgent>();
			_playerCollider = _player.gameObject.GetComponent<BoxCollider>();
			_attackTrigger  = GetComponent<BoxCollider>();
		}

		private void Update() 
		{ 
			if (Mouse.current.rightButton.isPressed)
				Execute(); 
		}
		
		public void Initialize(float damage)
		{
			_damage = damage;
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
			if (Time.time < _nextAttack)
				return;

			_playerCollider.enabled      = false;
			_agent.velocity              = Vector3.zero;
			PlayerController.IsAttacking = true;
			ServiceLocator.HUD.SpecialAttack?.StartCooldown(_player.AbilityCooldownTime);
			_nextAttack = Time.time + _player.AbilityCooldownTime;
			
			StartCoroutine(WindDash());
		}

		private IEnumerator WindDash()
		{
			Time.timeScale = 0.5f;
			float   time          = 0;
			float   duration      = 0.03f;
			while (time < duration) {
				float smoothStepLerp = time / duration;
				smoothStepLerp = smoothStepLerp * smoothStepLerp * (3f - 2f * smoothStepLerp);
				_agent.Move(transform.forward * Mathf.Lerp(5f, 0f, smoothStepLerp));
				time           += Time.deltaTime;
				yield return null;
			}
			_attackTrigger.enabled = true;
			yield return new WaitForSeconds(0.1f);
			ResetDashingModifications();
		}

		private void ResetDashingModifications()
		{
			_attackTrigger.enabled       = false;
			_playerCollider.enabled      = true;
			PlayerController.IsAttacking = false;
			Time.timeScale               = 1f;
		} 
	}
}