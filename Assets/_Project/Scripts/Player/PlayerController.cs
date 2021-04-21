using System;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Managers;
using _Project.Scripts.Player.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class PlayerController : MonoBehaviour
	{
		[Header("ACTIVE SETTINGS/STATS:")]
		[SerializeField] private PlayerSettings             baseSettings;
		[SerializeField] private ElementalPlayerStats       currentElementalStats;
		
		[SerializeField] private ElementalPlayerStats[] elementalStats = new ElementalPlayerStats[5];

		private PlayerInput                _input;
		private PlayerAim                  _aim;
		private PlayerMove                 _move;
		private PlayerShoot                _shoot;
		private PlayerSpecialAttack        _specialAttack;
		private ElementalSystemTypeCurrent _elementType;

		public static float EnemyDamage;
		public static float PlayerDamage;
		public static float PlayerDamageOverTime;
		public static float DamageOverTimeRate;
		public static bool  IsDealingDamageOverTime;

		private float _moveSpeed;
		private float _attackRate;
		private float _projectileSpeed;
		private float _specialAttackRate;

		private void Awake()
		{
			GetComponentReferences();
			SetStartingPlayerStats();
		}

		private void Update()
		{
			_aim.Aim(_input.AimDirection);
			
			if (_input.FireInput)
				_shoot.Fire(_attackRate, _projectileSpeed);
			
			if (_input.SpecialInput && currentElementalStats != elementalStats[4]) 
				_specialAttack.Activate(currentElementalStats.specialAttack, _projectileSpeed, _specialAttackRate, _elementType);
		}

		private void FixedUpdate()
		{
			_move.Move(_input.MoveDirection, _moveSpeed);
		}
		
		public void SwitchElementalStats()
		{
			currentElementalStats = _elementType.Type switch {
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
			DamageOverTimeRate   = baseSettings.damageOverTimeRate;
			PlayerDamageOverTime = baseSettings.attackStrength * baseSettings.damageOverTimeMultiplier;
			_projectileSpeed     = baseSettings.projectileSpeed;
			_specialAttackRate   = baseSettings.specialAttackRate;
			SetElementBasedPlayerStats();
		}
		
		private void SetElementBasedPlayerStats()
		{
			EnemyDamage             = baseSettings.damageReceived * currentElementalStats.damageReceivedMultiplier;
			PlayerDamage            = baseSettings.attackStrength * currentElementalStats.attackStrengthMultiplier;
			_moveSpeed              = baseSettings.moveSpeed      * currentElementalStats.moveSpeedMultiplier;
			_attackRate             = baseSettings.attackRate     / currentElementalStats.attackRateMultiplier;
			IsDealingDamageOverTime = currentElementalStats.isDealingDamageOverTime;
		}

		public void UpdateHealthBar(float remainingPercent) => ServiceLocator.HUD.Healthbar.UpdateHealthBar(remainingPercent);
	}
}