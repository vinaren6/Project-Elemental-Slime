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
		[SerializeField] private ElementalPlayerStats earth;
		[SerializeField] private ElementalPlayerStats wind;
		[SerializeField] private ElementalPlayerStats water;
		[SerializeField] private ElementalPlayerStats fire;
		[SerializeField] private ElementalPlayerStats noElement;
		
		private PlayerInput                _input;
		private PlayerAim                  _aim;
		private PlayerMove                 _move;
		private PlayerShoot                _shoot;
		private ElementalSystemTypeCurrent elementType;
		
		public static float EnemyDamage;
		public static float PlayerDamage;
		public static float DamageOverTimeRate;
		public static float DamageOverTimeMultiplier;
		public static bool  IsDealingDamageOverTime;

		private float _moveSpeed;
		private float _attackRate;
		private float _projectileSpeed;

		private void Awake()
		{
			_input      = GetComponent<PlayerInput>();
			_aim        = GetComponent<PlayerAim>();
			_move       = GetComponent<PlayerMove>();
			_shoot      = GetComponent<PlayerShoot>();
			elementType = GetComponent<ElementalSystemTypeCurrent>();
			SetPlayerStats();
		}
		
		private void Update()
		{
			_aim.Aim(_input.AimDirection);
			if (_input.FireInput)
				_shoot.Fire(_attackRate, _projectileSpeed);
		}

		private void FixedUpdate()
		{
			_move.Move(_input.MoveDirection, _moveSpeed);
		}
		
		public void SwitchElementalStats()
		{
			currentElementalStats = elementType.Type switch {
				ElementalSystemTypes.Earth => earth,
				ElementalSystemTypes.Wind  => wind,
				ElementalSystemTypes.Water => water,
				ElementalSystemTypes.Fire  => fire,
				ElementalSystemTypes.Base  => noElement,
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
			IsDealingDamageOverTime  = currentElementalStats.isDealingDamageOverTime;
		}
	}
}