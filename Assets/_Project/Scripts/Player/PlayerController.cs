using System;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using _Project.Scripts.Managers;
using _Project.Scripts.Player.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class PlayerController : MonoBehaviour
	{
		[Header("ACTIVE SETTINGS/STATS:")]
		[SerializeField] private PlayerSettings         baseSettings;
		[SerializeField] private PlayerElementalStats   currentPlayerElementalStats;
		[SerializeField] private PlayerElementalStats[] elementalStats = new PlayerElementalStats[5];
		
		[Header("ADDITIONAL MOVEMENT SETTINGS:")]
		[SerializeField][Range(0.25f, 1.0f)] private float moveWhenAttackingMultiplier = 1.0f;
		[SerializeField][Range(0.25f, 1.0f)] private float moveBackwardsMultiplier     = 1.0f;
		
		private PlayerInput                _input;
		private PlayerAim                  _aim;
		private PlayerMove                 _move;
		private PlayerShoot                _shoot;
		private PlayerSpecialAttack        _specialAttack;
		private ElementalSystemTypeCurrent _elementType;

		public static float EnemyDamageMultiplier;
		public static float PlayerDamage;
		public static float PlayerDamageOverTime;
		public static float DamageOverTimeCooldownTime;
		public static int   DamageOverTimeTotalTicks;
		public static bool  IsDealingDamageOverTime;

		private float _moveSpeed;
		private float _attackCooldownTime;
		private float _projectileSpeed;
		private float _specialAttackCooldownTime;

		private void Awake()
		{
			GetComponentReferences();
			SetStartingPlayerStats();
		}

		private void Update()
		{
			if (ServiceLocator.Game.IsPaused)
				return;

			_move.Move(_input.MoveDirection, _moveSpeed, moveBackwardsMultiplier);
			
			_aim.Aim(_input.AimDirection);
			
			if (_input.FireInput)
				_shoot.Fire(_attackCooldownTime, _projectileSpeed, moveWhenAttackingMultiplier);
			
			if (_input.SpecialInput && currentPlayerElementalStats != elementalStats[4]) 
				_specialAttack.Activate(currentPlayerElementalStats.specialAttack, _projectileSpeed, _specialAttackCooldownTime, _elementType);
		}

		public void SwitchElementalStats()
		{
			currentPlayerElementalStats = _elementType.Type switch {
				ElementalSystemTypes.Earth => elementalStats[0],
				ElementalSystemTypes.Wind  => elementalStats[1],
				ElementalSystemTypes.Water => elementalStats[2],
				ElementalSystemTypes.Fire  => elementalStats[3],
				ElementalSystemTypes.Base  => elementalStats[4],
				_                          => throw new ArgumentOutOfRangeException(nameof(_elementType.Type), _elementType.Type, null)
			};
			SetElementBasedPlayerStats();
			ServiceLocator.HUD.UpdateElementBar(_elementType.Type, 0f);
		}
		
		private void GetComponentReferences()
		{
			_input         = GetComponent<PlayerInput>();
			_aim           = GetComponent<PlayerAim>();
			_move          = GetComponent<PlayerMove>();
			_shoot         = GetComponent<PlayerShoot>();
			_specialAttack = GetComponent<PlayerSpecialAttack>();
			_elementType   = GetComponent<ElementalSystemTypeCurrent>();
		}
		
		private void SetStartingPlayerStats()
		{
			DamageOverTimeCooldownTime          = baseSettings.damageOverTimeCooldownTime;
			PlayerDamageOverTime                = baseSettings.attackStrength * baseSettings.damageOverTimeMultiplier;
			DamageOverTimeTotalTicks            = baseSettings.damageOverTimeTotalTicks;
			_projectileSpeed                    = baseSettings.projectileSpeed;
			_specialAttackCooldownTime          = baseSettings.specialAttackCooldownTime;
			GetComponent<Health>().MaxHitPoints = baseSettings.maxHitPoints;
			
			SetElementBasedPlayerStats();
		}
		
		private void SetElementBasedPlayerStats()
		{
			EnemyDamageMultiplier   = currentPlayerElementalStats.damageReceivedMultiplier;
			PlayerDamage            = baseSettings.attackStrength * currentPlayerElementalStats.attackStrengthMultiplier;
			_moveSpeed              = baseSettings.moveSpeed      * currentPlayerElementalStats.moveSpeedMultiplier;
			_attackCooldownTime     = baseSettings.attackCooldownTime / currentPlayerElementalStats.attackRateMultiplier;
			IsDealingDamageOverTime = currentPlayerElementalStats.isDealingDamageOverTime;
		}

		public void UpdateHealthBar(float remainingPercent) => ServiceLocator.HUD.Healthbar.UpdateHealthBar(remainingPercent);
	}
}