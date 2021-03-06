using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.VFX;

namespace _Project.Scripts.Abilities
{
	public class FireAbility : MonoBehaviour, IAbility
	{
		#region Variables
		
		[SerializeField] private GameObject flamePrefab;
		[SerializeField] private Transform  flamePoolTransform;
		[SerializeField] private AudioClip  fireSFX;

		[Header("Test Values")]
		[SerializeField] [Range(1f, 150f)]    private float speed;
		[SerializeField] [Range(1.0f, 1.20f)] private float speedMultiplier;
		[SerializeField] [Range(0.1f, 0.5f)]  private float aliveTime;

		private Transform _transform;
		private NavMeshAgent         _agent;
		private Queue<FlameCollider> _flamePool;
		private VisualEffect         _effect;
		private AudioSource          _audioSource;

		private int   _maxFlameColliders;
		private float _fireRate;
		private bool  _canShoot;

		private float _damage;
		
		public bool StopLooping => false;
		
		#endregion

		#region Start Methods

		private void Awake()
		{
			_transform = transform;
			_agent     = GetComponentInParent<NavMeshAgent>();
			_effect    = GetComponentInChildren<VisualEffect>();
			_audioSource = gameObject.AddComponent<AudioSource>();
		}

		public void Initialize(string newTag, float damage, Collider selfCollider = null)
		{
			tag = newTag;
			_damage = damage;
			_maxFlameColliders = 30;
			_fireRate = 1f / _maxFlameColliders;
			_canShoot = true;
			_effect.Stop();
			_audioSource.outputAudioMixerGroup = Resources.Load<AudioMixer>("AudioMixer").FindMatchingGroups("SFX")[0];
			_audioSource.loop = true;
			_audioSource.volume = 1.5f;
			_audioSource.clip = fireSFX;
			
			
			if (_flamePool != null)
				return;
			
			_flamePool = new Queue<FlameCollider>();
			
			for (int i = 0; i < _maxFlameColliders; i++)
			{
				GameObject flameObject = Instantiate(flamePrefab, flamePoolTransform);
				FlameCollider flameCollider = flameObject.GetComponent<FlameCollider>();
				flameCollider.Initialize(tag, this, _damage);
				flameObject.SetActive(false);
				
				_flamePool.Enqueue(flameCollider);
			}
		}

		public void Initialize(string newTag, float damage, float distance, Collider selfCollider = null)
		{
			Initialize(newTag, damage, selfCollider);
		}

		#endregion

		#region Methods

		public bool DidExecute()
		{
			if (!_audioSource.isPlaying)
			{
				_audioSource.Play();
			}
			if (!_canShoot)
				return false;
			
			_effect.Play();
			
			float speedAdjustment = GetLookDirectionSpeedAdjustment();

			FlameCollider flameCollider = _flamePool.Dequeue();

			flameCollider.gameObject.SetActive(true);
			flameCollider.Execute(_transform, speed * speedAdjustment, speedMultiplier, aliveTime);

			_effect.playRate = speedAdjustment * 2f;

			_canShoot = false;
			StartCoroutine(nameof(SpawnDelayRoutine));

			if (CompareTag("Enemy"))
				_agent.speed = 5f;

			return true;
		}

		public void Stop(bool isPlayer = true)
		{
			if (CompareTag("Enemy"))
				_agent.speed = 14f;
			
			_effect.Stop();
			_audioSource.Stop();
		}

		public bool IsInRange()
		{
			// TODO Better calc for checking range.

			PlayerController controller = FindObjectOfType<PlayerController>();
			
			if (!controller)
			{
				return false;
			}
			Transform p = controller.transform;
			bool IsInRange = Vector3.Distance(_transform.position, p.position) < 10f;
			if (IsInRange)
			{
				walkTowardPlayer();
			}
			return IsInRange;
		}
		private void walkTowardPlayer()
        {
			PlayerController controller = FindObjectOfType<PlayerController>();
			Transform p = controller.transform;
			// Debug.Log("walktowardPlayer");
			
			var turnSpeed = 10.0f;
			var _dir = GameObject.FindObjectOfType<PlayerController>().transform.position - _transform.parent.gameObject.transform.position;
			_dir.Normalize();
			_transform.parent.gameObject.transform.rotation = Quaternion.Slerp(_transform.rotation, Quaternion.LookRotation(_dir), turnSpeed * Time.deltaTime);
			if (Vector3.Distance(_transform.position, p.position) > 2f)
            {
				_agent.SetDestination(controller.transform.position);
            }
            else
            {
				_agent.SetDestination(_transform.parent.gameObject.transform.position);
			}
        }
		public bool IsInWalkRange()
		{
			// TODO Better calc for checking range.

			PlayerController controller = FindObjectOfType<PlayerController>();

			if (!controller)
			{
				return false;
			}
			Transform p = controller.transform;
			bool IsInRange = Vector3.Distance(_transform.position, p.position) < 5f;
			
			return IsInRange;
		}

		public bool CanAttack()
		{
			return _canShoot;
		}

		public float GetAttackTime()
		{
			return _fireRate;
		}

		private IEnumerator SpawnDelayRoutine()
		{
			yield return new WaitForSeconds(_fireRate);
			_canShoot = true;
		}

		public void ReturnToPool(FlameCollider flameObject)
		{
			flameObject.gameObject.SetActive(false);
			flameObject.transform.SetParent(flamePoolTransform);

			_flamePool.Enqueue(flameObject);
		}
		
		private float GetLookDirectionSpeedAdjustment()
		{
			Vector3 moveDirection      = _agent.velocity;
			float   moveLookAdjustment = (Vector3.Dot(_transform.forward, moveDirection.normalized) / 2.5f);
			moveLookAdjustment += 1;
			// print("MovelookAjustment:" + moveLookAdjustment);
			return moveLookAdjustment;
		}
		
		#endregion
	}
}