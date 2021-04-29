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
	public class EarthAbility : MonoBehaviour, IAbility
	{
		private PlayerController           _player;
		private NavMeshAgent               _agent;
		private Transform                  _transform;
		private BoxCollider                _attackTrigger;
		private ParticleSystem             _earthEffect;
		private ParticleSystem.ShapeModule _effectShape;
		private Vector3                    _startPosition;
		private Vector3                    _startSize;
		private Transform                  _playerParent;
		private float                      _nextAttack = 0;

		private void Awake()
		{
			_player        = GetComponentInParent<PlayerController>();
			_agent         = GetComponentInParent<NavMeshAgent>();
			_transform     = gameObject.transform;
			_attackTrigger = GetComponent<BoxCollider>();
			_earthEffect   = GetComponentInChildren<ParticleSystem>();
			_effectShape   = _earthEffect.shape;
			_startPosition = _attackTrigger.transform.localPosition;
			_startSize     = _attackTrigger.size;
			_playerParent  = transform.parent;

			_earthEffect.Stop();
		}

		private void Update() 
		{ 
			if (Mouse.current.rightButton.isPressed)
				Execute(); 
		}

		private void OnDisable() => ResetEarthQuakeModifications();

		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent(out IHealth health)) {
				float earthMultiplier = Mathf.Clamp(_attackTrigger.size.x, 5f, 10f) / 10;
				health.ReceiveDamage(ElementalSystemTypes.Earth, earthMultiplier * PlayerController.PlayerDamage * _player.SpecialAttackMultiplier);
				other.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
			}
		}

		public void Initialize(float damage) {
			
		}
		
		public void Execute()
		{
			if (Time.time < _nextAttack)
				return;
			
			_agent.velocity  = Vector3.zero;
			_player.IsAttacking = true;
			PlayEffect();
			ServiceLocator.HUD.SpecialAttack?.StartCooldown(_player.SpecialAttackCooldownTime);
			_nextAttack = Time.time + _player.SpecialAttackCooldownTime;
			StartCoroutine(EarthQuake());
		}

		private IEnumerator EarthQuake()
		{
			_attackTrigger.enabled = true;
			transform.SetParent(null);
			
			float   time              = 0;
			float   duration          = 1.5f;
			
			while (time < duration) {
				float smoothStepLerp = time / duration;
				smoothStepLerp      =  smoothStepLerp     * smoothStepLerp * (3f - 2f * smoothStepLerp);
				_transform.position += _transform.forward * Mathf.Lerp(0.2f, 0, smoothStepLerp);
				_attackTrigger.size = new Vector3((Mathf.Lerp(2f, 20f, smoothStepLerp)), _startSize.y, _startSize.z);
				_effectShape.scale  = new Vector3((Mathf.Lerp(2.5f, 22f, smoothStepLerp)), 1, 1);
				
				if (time > duration / 3)
					_player.IsAttacking = false;
				
				time += Time.deltaTime;
				yield return null;
			}
			yield return new WaitForSeconds(0.25f);
			_earthEffect.Stop();
			yield return new WaitForSeconds(0.25f);
			ResetEarthQuakeModifications();
		}
		
		private void ResetEarthQuakeModifications()
		{
			_earthEffect.Stop();
			transform.SetParent(_playerParent);
			_attackTrigger.enabled      = false;
			_transform.localPosition    = _startPosition;
			_transform.localEulerAngles = Vector3.zero;
			_attackTrigger.size         = _startSize;
			_effectShape.scale          = new Vector3(2, 1, 1);
			_player.IsAttacking           = false;
		}
		
		private void PlayEffect()
		{
			if (_earthEffect.isPlaying)
				return;
			_earthEffect.Play();
		}
	}
}