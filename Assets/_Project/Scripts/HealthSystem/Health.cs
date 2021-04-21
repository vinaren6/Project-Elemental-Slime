using System;
using _Project.Scripts.ElementalSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.HealthSystem
{
	public class Health : MonoBehaviour, IHealth
	{
		public static event Action<Vector3, int, float> onAnyReceiveDamage;

		[SerializeField] private ElementalSystemTypeCurrent type;
		[SerializeField] private float                      maxHitPoints;
		public                   UnityEvent<float>          onReceiveDamage;
		public                   UnityEvent                 onDeath;
		public                   float                      HitPoints { get; set; }

		public float MaxHitPoints {
			get => maxHitPoints;
			set => maxHitPoints = value;
		}
		public float RemainingPercent => HitPoints / maxHitPoints;

		private void Start() => HitPoints = maxHitPoints;

		public void ReceiveDamage(ElementalSystemTypes damageType, float damage)
		{
			float elementalMultiplier = ElementalSystemMultiplier.GetMultiplier(type.Type, damageType);
			int   damageToReceive     = Mathf.CeilToInt(elementalMultiplier * damage);
			HitPoints -= damageToReceive;
			onAnyReceiveDamage?.Invoke(transform.position, damageToReceive, elementalMultiplier);
			onReceiveDamage.Invoke(RemainingPercent);
			if (!(HitPoints <= 0)) return;
			onDeath.Invoke();
			Destroy(this);
		}

		public void ReceiveHealth(ElementalSystemTypes damageType, float damage) =>
			HitPoints = Mathf.Min(
				maxHitPoints, ElementalSystemMultiplier.GetMultiplier(type.Type, damageType) * damage);
	}
}