using System.Collections;
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
		private float            _nextAttack = 0;

		private void Awake()
		{
			_player                     = GetComponentInParent<PlayerController>();
			_agent                      = GetComponentInParent<NavMeshAgent>();
			//Application.targetFrameRate = 300;
		}

		private void Update() 
		{ 
			if (Mouse.current.leftButton.isPressed)
				Execute(); 
		}

		public void Execute()
		{
			if (!(Time.time > _nextAttack))
				return;
            
			_agent.velocity   = Vector3.zero;
			Time.timeScale    = 0.5f;
			_player.isDashing = true;
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

			yield return new WaitForSeconds(0.1f);
			Time.timeScale    = 1f;
			_player.isDashing = false;
		}
	}
}