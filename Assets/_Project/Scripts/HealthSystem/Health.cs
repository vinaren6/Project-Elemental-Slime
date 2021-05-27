using System;
using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Enemies;
using _Project.Scripts.Managers;
using _Project.Scripts.UI;
using _Project.Scripts.UI.Score;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.HealthSystem
{
	public class Health : MonoBehaviour, IHealth
	{
		[SerializeField] private ElementalSystemTypeCurrent type;
		[SerializeField] private AudioClip                  hurtSFX;
		[SerializeField] private AudioClip                  deathSFX;
		
		public UnityEvent<float> onReceiveDamage;
		public UnityEvent<float> onReceiveHealth;
		public UnityEvent        onDeath;
		
		private float _canReceiveDamageCooldownTime = 0.1f;
		private bool  _canReceiveDamage             = true;
		private float _maxHitPoints;

		public float MaxHitPoints {
			get => _maxHitPoints;
			set => _maxHitPoints = value;
		}
		public  float HitPoints        { get; set; }
		public  float NormalizedHealth => HitPoints / _maxHitPoints;
		private void  Start()          => HitPoints = _maxHitPoints;

		public void ReceiveDamage(ElementalSystemTypes damageType, float damage)
		{
			if (!_canReceiveDamage)
				return;

			StartCoroutine(ReceiveDamageCooldownTimeRoutine());
			
			float elementalMultiplier = ElementalSystemMultiplier.GetMultiplier(type.Type, damageType);
			int   damageToReceive     = Mathf.CeilToInt(elementalMultiplier * damage);
			
			HitPoints -= damageToReceive;
			
			ServiceLocator.Pools.SpawnFromPool(
				PoolType.DamageNumber, transform.position, damageToReceive, GetEffectiveType(elementalMultiplier), GetDamageType());
			
			onReceiveDamage.Invoke(NormalizedHealth);
			
			if (!(HitPoints <= 0))
			{
				if (hurtSFX == null)
					throw new AggregateException("audioClip reference not set to an instance of an object");
				
				ServiceLocator.Audio.PlaySFX(hurtSFX, 0.4f);
				return;
			}
			
			if (deathSFX == null)
				throw new AggregateException("audioClip reference not set to an instance of an object");
			
			ServiceLocator.Audio.PlaySFX(deathSFX, 0.7f);
			EnemySpawner.EnemiesInScene--;
			
			onDeath.Invoke();
		}

		public void CallEnemyDeath() => ScoreController.GiveScore(ScoreType.EnemyKill, transform.position);

		private IEnumerator ReceiveDamageCooldownTimeRoutine()
		{
			_canReceiveDamage = false;
			yield return new WaitForSeconds(_canReceiveDamageCooldownTime);
			_canReceiveDamage = true;
		}

		public void ReceiveHealth(ElementalSystemTypes regenType, float hpRegain)
		{
			if (HitPoints == _maxHitPoints) return;
			float elementalMultiplier = ElementalSystemMultiplier.GetMultiplier(type.Type, regenType);
			float hpToReceive = Mathf.Min(
				_maxHitPoints - HitPoints, elementalMultiplier * hpRegain);
			HitPoints += hpToReceive;
			ServiceLocator.Pools.SpawnFromPool(
				PoolType.DamageNumber, transform.position, (int) hpToReceive, EffectiveType.Heal, DamageType.Heal);
			onReceiveHealth.Invoke(NormalizedHealth);
		}
		
		private EffectiveType GetEffectiveType(float elementalMultiplier)
		{
			if (elementalMultiplier < 1)
				return EffectiveType.Weak;
			if (elementalMultiplier > 1)
				return EffectiveType.Effective;
			return EffectiveType.Neutral;
		}

		private DamageType GetDamageType()
		{
			return CompareTag("Player") ? DamageType.Player : DamageType.Enemy;
		}

		public void KillPlayer()
		{
			onDeath.Invoke();
		}
	}
}