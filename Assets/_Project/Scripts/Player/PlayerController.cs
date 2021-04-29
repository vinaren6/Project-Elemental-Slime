using System;
using _Project.Scripts.Abilities;
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
		[SerializeField][Range(0.1f, 1.0f)]  private float moveSmoothing               = 0.9f;
		[SerializeField][Range(1.0f, 15.0f)] private float rotationSmoothing           = 7.0f;
		[SerializeField][Range(0.1f, 1.0f)]  private float moveWhenAttackingMultiplier = 1.0f;
		[SerializeField][Range(0.1f, 1.0f)]  private float moveBackwardsMultiplier     = 1.0f;
		
		private PlayerInput                _input;
		private PlayerAim                  _aim;
		private PlayerMove                 _move;
		private PlayerShoot                _shoot;
		private PlayerSpecialAttack        _specialAttack;
		private ElementalSystemTypeCurrent _elementType;
		private IAbility                   _ability;

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

		public float MoveSpeed                 => _moveSpeed;
		public float AttackCooldownTime        => _attackCooldownTime;
		public float ProjectileSpeed           => _projectileSpeed;
		public float SpecialAttackMultiplier   { get; } = 5f;
		public float SpecialAttackCooldownTime => _specialAttackCooldownTime;
		public bool  HasAttacked               { get; set; }
		public bool  IsAttacking               { get; set; }

		private void Awake()
		{
			GetComponentReferences();
			SetBaseStats();
		}

		private void Update()
		{
			if (ServiceLocator.Game.IsPaused)
				return;

			if (IsAttacking)
				return;
				
			_move.Move(_input.MoveDirection, moveSmoothing, moveBackwardsMultiplier, moveWhenAttackingMultiplier);
			_aim.Aim(_input.AimDirection, rotationSmoothing);

			if (_input.FireInput)
				_shoot.Fire();
			
			// if (_input.SpecialInput && currentPlayerElementalStats != elementalStats[(int)ElementalSystemTypes.Base]) 
			// 	_specialAttack.Activate(currentPlayerElementalStats.specialAttack, _projectileSpeed, _specialAttackCooldownTime, _elementType);
		}

		#region Methods
		
		private void GetComponentReferences()
		{
			_input         = GetComponent<PlayerInput>();
			_aim           = GetComponent<PlayerAim>();
			_move          = GetComponent<PlayerMove>();
			_shoot         = GetComponent<PlayerShoot>();
			_specialAttack = GetComponent<PlayerSpecialAttack>();
			_elementType   = GetComponent<ElementalSystemTypeCurrent>();
		}
		
		private void SetBaseStats()
		{
			DamageOverTimeCooldownTime          = baseSettings.damageOverTimeCooldownTime;
			PlayerDamageOverTime                = baseSettings.attackStrength * baseSettings.damageOverTimeMultiplier;
			DamageOverTimeTotalTicks            = baseSettings.damageOverTimeTotalTicks;
			_projectileSpeed                    = baseSettings.projectileSpeed;
			_specialAttackCooldownTime          = baseSettings.specialAttackCooldownTime;
			GetComponent<Health>().MaxHitPoints = baseSettings.maxHitPoints;
			
			SetElementBasedStats();
		}
		
		private void SetElementBasedStats()
		{
			EnemyDamageMultiplier   = currentPlayerElementalStats.damageReceivedMultiplier;
			PlayerDamage            = baseSettings.attackStrength * currentPlayerElementalStats.attackStrengthMultiplier;
			_moveSpeed              = baseSettings.moveSpeed      * currentPlayerElementalStats.moveSpeedMultiplier;
			_attackCooldownTime     = baseSettings.attackCooldownTime / currentPlayerElementalStats.attackRateMultiplier;
			IsDealingDamageOverTime = currentPlayerElementalStats.isDealingDamageOverTime;
		}
		
		public void SwitchElementalStats()
		{
			currentPlayerElementalStats = _elementType.Type switch {
				ElementalSystemTypes.Earth => elementalStats[(int)ElementalSystemTypes.Earth],
				ElementalSystemTypes.Wind  => elementalStats[(int)ElementalSystemTypes.Wind],
				ElementalSystemTypes.Water => elementalStats[(int)ElementalSystemTypes.Water],
				ElementalSystemTypes.Fire  => elementalStats[(int)ElementalSystemTypes.Fire],
				ElementalSystemTypes.Base  => elementalStats[(int)ElementalSystemTypes.Base],
				_                          => throw new ArgumentOutOfRangeException(nameof(_elementType.Type), _elementType.Type, null)
			};
			SetElementBasedStats();
			ServiceLocator.HUD.UpdateElementBar(_elementType.Type, 0f);
		}

		public void UpdateHealthBar(float remainingPercent) => ServiceLocator.HUD.Healthbar.UpdateHealthBar(remainingPercent);
		
		#endregion

	}
}