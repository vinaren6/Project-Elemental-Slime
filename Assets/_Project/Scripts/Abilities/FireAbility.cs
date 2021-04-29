using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Abilities
{
	public class FireAbility : MonoBehaviour, IAbility
	{
		[SerializeField] private GameObject flamePrefab;
		[SerializeField] private Transform flamePoolTransform;

		private Queue<GameObject> _flamePool;
		
		private int _maxFlameColliders;
		private float _fireRate;

		private float _damage;

		private void Awake()
		{
			Initialize(1f);
		}

		private void Update()
		{
			if (Mouse.current.rightButton.isPressed)
				Execute();
		}

		public void Initialize(float damage)
		{
			_damage = damage;
			_maxFlameColliders = 3;
			_fireRate = 1f;

			_flamePool = new Queue<GameObject>();

			for (int i = 0; i < _maxFlameColliders; i++)
			{
				GameObject FlameObject = Instantiate(flamePrefab, flamePoolTransform);
				FlameObject.GetComponent<FlameCollider>().Initialize(_damage);
				FlameObject.SetActive(false);
				
				_flamePool.Enqueue(FlameObject);
			}
		}

		public void Execute()
		{
		}
	}
}