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
		[SerializeField] private Transform  flamePoolTransform;

		[Header("Test Values")]
		[SerializeField] [Range(1f, 150f)]    private float speed;
		[SerializeField] [Range(1.0f, 1.20f)] private float speedMultiplier;
		[SerializeField] [Range(0.1f, 0.5f)]  private float aliveTime;

		private NavMeshAgent         _agent;
		private Queue<FlameCollider> _flamePool;
		private VisualEffect         _effect;

		private int   _maxFlameColliders;
		private float _fireRate;
		private bool  _canShoot;

		private float _damage;
		
		public bool StopLooping => false;
		
		#endregion

		#region Start Methods

		private void Awake()
		{
			_agent     = GetComponentInParent<NavMeshAgent>();
			_effect    = GetComponentInChildren<VisualEffect>();
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

		public bool DidExecute()
		{
			if (!_canShoot)
				return false;
			
			_effect.Play();
			
			float speedAdjustment = GetLookDirectionSpeedAdjustment();

			FlameCollider flameCollider = _flamePool.Dequeue();

			flameCollider.gameObject.SetActive(true);
			flameCollider.Execute(transform, speed * speedAdjustment, speedMultiplier, aliveTime);

			_effect.playRate = speedAdjustment * 2f;

			_canShoot = false;
			StartCoroutine(nameof(SpawnDelayRoutine));

			return true;
		}

		public void Stop(bool isPlayer = true)
		{
			_effect.Stop();
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

			return Vector3.Distance(transform.position, p.position) < 10f;
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

			return Vector3.Distance(transform.position, p.position) < 5f;
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
			float   moveLookAdjustment = (Vector3.Dot(transform.forward, moveDirection.normalized) / 2.5f);
			moveLookAdjustment += 1;
			// print("MovelookAjustment:" + moveLookAdjustment);
			return moveLookAdjustment;
		}
		
		#endregion
	}
}