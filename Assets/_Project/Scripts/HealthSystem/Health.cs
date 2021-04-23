using System;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Enemies;
using _Project.Scripts.Managers;
using _Project.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.HealthSystem
{
	public class Health : MonoBehaviour, IHealth
	{
		public static event Action onAnyDeath;
		
		[SerializeField] private ElementalSystemTypeCurrent type;
		[SerializeField] private float                      maxHitPoints;
		[SerializeField] private AudioClip hurtSFX;
		[SerializeField] private AudioClip deathSFX;
		
		public                   UnityEvent<float>          onReceiveDamage;
		public                   UnityEvent                 onDeath;
		public                   float                      RemainingPercent => HitPoints / maxHitPoints;

		private void  Start()   => HitPoints = maxHitPoints;
		public  float HitPoints { get; set; }

		public float MaxHitPoints {
			get => maxHitPoints;
			set => maxHitPoints = value;
		}

		public void ReceiveDamage(ElementalSystemTypes damageType, float damage)
		{
			float elementalMultiplier = ElementalSystemMultiplier.GetMultiplier(type.Type, damageType);
			int   damageToReceive     = Mathf.CeilToInt(elementalMultiplier * damage);
			HitPoints -= damageToReceive;
			ServiceLocator.DamageNumbers.SpawnFromPool(
				transform.position, damageToReceive, GetEffectiveType(elementalMultiplier));
			onReceiveDamage.Invoke(RemainingPercent);
			if (!(HitPoints <= 0))
			{
				ServiceLocator.Audio.PlaySFX(hurtSFX);
				return;
			}
			ServiceLocator.Audio.PlaySFX(deathSFX);
			EnemySpawner.enemiesInScene--;
			onDeath.Invoke();
			onAnyDeath?.Invoke();
			Destroy(this);
		}

		public void ReceiveHealth(ElementalSystemTypes regenType, float hpRegain)
		{
			if (HitPoints == maxHitPoints) return;
			float elementalMultiplier = ElementalSystemMultiplier.GetMultiplier(type.Type, regenType);
			float hpToReceive = Mathf.Min(
				maxHitPoints - HitPoints, elementalMultiplier * hpRegain);
			HitPoints += hpToReceive;
			ServiceLocator.DamageNumbers.SpawnFromPool(
				transform.position, (int) hpToReceive, EffectiveType.Heal);
			onReceiveDamage.Invoke(RemainingPercent);
		}


		private EffectiveType GetEffectiveType(float elementalMultiplier)
		{
			if (elementalMultiplier < 1)
				return EffectiveType.Weakness;
			if (elementalMultiplier > 1)
				return EffectiveType.Effective;
			return EffectiveType.Neutral;
		}

		public void KillPlayer()
		{
			onDeath.Invoke();
		}
	}
}