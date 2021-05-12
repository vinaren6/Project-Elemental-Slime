using System;
using System.Collections;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Enemies;
using _Project.Scripts.Enemies.AI;
using _Project.Scripts.Managers;
using _Project.Scripts.Player;
using _Project.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.HealthSystem
{
	public class Health : MonoBehaviour, IHealth
	{
		public static event Action<Vector3> onAnyEnemyDeath;
		
		[SerializeField] private ElementalSystemTypeCurrent type;
		[SerializeField] private AudioClip                  hurtSFX;
		[SerializeField] private AudioClip                  deathSFX;
		
		public                   UnityEvent<float>          onReceiveDamage;
		public                   UnityEvent                 onDeath;
		
		private float _canReceiveDamageCooldownTime = 0.1f;
		private bool  _canReceiveDamage             = true;
		private float _maxHitPoints;
		public float MaxHitPoints {
			get => _maxHitPoints;
			set => _maxHitPoints = value;
		}
		public  float HitPoints        { get; set; }
		public  float RemainingPercent => HitPoints / _maxHitPoints;

		
		private void Start() => HitPoints = _maxHitPoints;

		public void ReceiveDamage(ElementalSystemTypes damageType, float damage)
		{
			if (!_canReceiveDamage)
				return;

			StartCoroutine(ReceiveDamageCooldownTimeRoutine());
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
			EnemySpawner.EnemiesInScene--;
			onDeath.Invoke();
		}

		public void CallEnemyDeath() => onAnyEnemyDeath?.Invoke(transform.position);

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
			ServiceLocator.DamageNumbers.SpawnFromPool(
				transform.position, (int) hpToReceive, EffectiveType.Heal);
			onReceiveDamage.Invoke(RemainingPercent);
		}
		
		private EffectiveType GetEffectiveType(float elementalMultiplier)
		{
			if (elementalMultiplier < 1)
				return EffectiveType.Weak;
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