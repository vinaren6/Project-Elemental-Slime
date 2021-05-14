using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

namespace _Project.Scripts.Abilities
{
	public class FireAbility : MonoBehaviour, IAbility
	{
		#region Variables
		
		[SerializeField] private GameObject flamePrefab;
		[SerializeField] private Transform flamePoolTransform;

		[Header("Test Values")]
		[SerializeField] [Range(1f, 150f)] private float speed;
		[SerializeField] [Range(1.0f, 1.20f)] private float speedMultiplier;
		[SerializeField] [Range(0.1f, 0.5f)] private float aliveTime;

		private NavMeshAgent         _agent;
		private Queue<FlameCollider> _flamePool;
		private VisualEffect         _effect;
		
		private int _maxFlameColliders;
		private float _fireRate;
		private bool _canShoot;

		private float _damage;
		
		public bool StopLooping => false;
		
		#endregion

		#region Start Methods

		private void Awake()
		{
			_agent  = GetComponentInParent<NavMeshAgent>();
			_effect = GetComponentInChildren<VisualEffect>();
		}

		public void Initialize(string newTag, float damage, Collider selfCollider = null)
		{
			tag = newTag;
			_damage = damage;
			_maxFlameColliders = 30;
			_fireRate = 1f / _maxFlameColliders;
			_canShoot = true;
			_effect.Stop();
			
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

		public void Execute()
		{
			if (!_canShoot)
				return;
			
			_effect.Play();
			
			float adjustedSpeed = speed * GetLookDirectionSpeedAdjustment();

			FlameCollider flameCollider = _flamePool.Dequeue();

			flameCollider.gameObject.SetActive(true);
			flameCollider.Execute(transform, adjustedSpeed, speedMultiplier, aliveTime);

			_canShoot = false;
			StartCoroutine(nameof(SpawnDelayRoutine));
		}

		public void Stop(bool isPlayer = true)
		{
			_effect.Stop();
		}

		public bool IsInRange()
		{
			// TODO Better calc for checking range.
			
			Transform p = GameObject.FindObjectOfType<PlayerController>().transform;

			return Vector3.Distance(transform.position, p.position) < 10f;
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
			float   moveLookAdjustment = (Vector3.Dot(Vector3.forward, moveDirection.normalized) / 2.5f);
			moveLookAdjustment += 1;
			// print("MovelookAjustment:" + moveLookRelation);
			return moveLookAdjustment;
		}
		
		#endregion
	}
}