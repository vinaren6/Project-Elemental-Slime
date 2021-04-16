using _Project.Scripts.ElementalSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.HealthSystem
{
	public class Health : MonoBehaviour, IHealth
	{
		[SerializeField] private ElementalSystemTypeCurrent type;
		[SerializeField] private float                      maxHitPoints;
		public                   UnityEvent                 onDeath;
		public                   float                      HitPoints { get; set; }

		public float MaxHitPoints {
			get => maxHitPoints;
			set => maxHitPoints = value;
		}

		private void Start() => HitPoints = maxHitPoints;

		public void ReceiveDamage(ElementalSystemTypes damageType, float damage)
		{
			HitPoints -= ElementalSystemMultiplier.GetMultiplier(type.Type, damageType) * damage;
			if (!(HitPoints <= 0)) return;
			onDeath.Invoke();
			Destroy(this);
		}

		public void ReceiveHealth(ElementalSystemTypes damageType, float damage) =>
			HitPoints = Mathf.Min(
				maxHitPoints, ElementalSystemMultiplier.GetMultiplier(type.Type, damageType) * damage);
	}
}