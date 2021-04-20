using System;
using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Player.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class PlayerController : MonoBehaviour
	{
		[Header("ACTIVE SETTINGS/STATS:")]
		[SerializeField] private PlayerSettings             baseSettings;
		[SerializeField] private ElementalPlayerStats       currentElementalStats;

		[Header("ELEMENTAL STATS:")]
		[SerializeField] private ElementalPlayerStats earthStats;
		[SerializeField] private ElementalPlayerStats windStats;
		[SerializeField] private ElementalPlayerStats waterStats;
		[SerializeField] private ElementalPlayerStats fireStats;
		[SerializeField] private ElementalPlayerStats baseStats;
		
		private PlayerInput                _input;
		private PlayerAim                  _aim;
		private PlayerMove                 _move;
		private PlayerShoot                _shoot;
		private PlayerSpecialAttack        _specialAttack;
		private ElementalSystemTypeCurrent _elementType;
		
		public static float EnemyDamage;
		public static float PlayerDamage;
		public static float DamageOverTimeRate;
		public static float DamageOverTimeMultiplier;
		public static bool  IsDealingDamageOverTime;

		private float _moveSpeed;
		private float _attackRate;
		private float _projectileSpeed;
		private float _specialAttackRate;

		private void Awake()
		{
			_input         = GetComponent<PlayerInput>();
			_aim           = GetComponent<PlayerAim>();
			_move          = GetComponent<PlayerMove>();
			_shoot         = GetComponent<PlayerShoot>();
			_specialAttack = GetComponent<PlayerSpecialAttack>();
			_elementType   = GetComponent<ElementalSystemTypeCurrent>();
			SetPlayerStats();
		}
		
		private void Update()
		{
			_aim.Aim(_input.AimDirection);
			
			if (_input.FireInput)
				_shoot.Fire(_attackRate, _projectileSpeed);
			
			if (_input.SpecialInput && currentElementalStats != baseStats) 
				_specialAttack.Activate(currentElementalStats.specialAttack, _projectileSpeed, _specialAttackRate, _elementType);
		}

		private void FixedUpdate()
		{
			_move.Move(_input.MoveDirection, _moveSpeed);
		}
		
		public void SwitchElementalStats()
		{
			currentElementalStats = _elementType.Type switch {
				ElementalSystemTypes.Earth => earthStats,
				ElementalSystemTypes.Wind  => windStats,
				ElementalSystemTypes.Water => waterStats,
				ElementalSystemTypes.Fire  => fireStats,
				ElementalSystemTypes.Base  => baseStats,
				_                          => throw new ArgumentOutOfRangeException()
			};
			SetPlayerStats();
		}
		
		private void SetPlayerStats()
		{
			EnemyDamage              = baseSettings.damageReceived * currentElementalStats.damageReceivedMultiplier;
			PlayerDamage             = baseSettings.attackStrength * currentElementalStats.attackStrengthMultiplier;
			DamageOverTimeRate       = baseSettings.damageOverTimeRate;
			DamageOverTimeMultiplier = baseSettings.damageOverTimeMultiplier;
			_moveSpeed               = baseSettings.moveSpeed  * currentElementalStats.moveSpeedMultiplier;
			_attackRate              = baseSettings.attackRate / currentElementalStats.attackRateMultiplier;
			_projectileSpeed         = baseSettings.projectileSpeed;
			_specialAttackRate       = baseSettings.specialAttackRate;
			IsDealingDamageOverTime  = currentElementalStats.isDealingDamageOverTime;
		}
	}
}