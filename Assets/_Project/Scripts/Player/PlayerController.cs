using System;
using System.Collections;
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
		[SerializeField] private GameObject[]           abilities      = new GameObject[5];
		[SerializeField] private GameObject             switchFX;
		[SerializeField] private Renderer               meshRenderer;

		[Header("ADDITIONAL MOVEMENT SETTINGS:")]
		[SerializeField][Range(0.1f, 1.0f)] private float moveSmoothing = 0.9f;
		[SerializeField][Range(0.1f, 1.0f)] private float moveBackwardsMultiplier = 1.0f;

		private PlayerInput                _input;
		private PlayerAim                  _aim;
		private PlayerMove                 _move;
		private PlayerShoot                _shoot;
		private ElementalSystemTypeCurrent _elementType;
		private Animator                   _animator;
		private Collider                   _collider;
		private IAbility                   _ability;
		
		public static           float PlayerDamage;
		public static           float PlayerDamageOverTime;
		public static           float DamageOverTimeCooldownTime;
		public static           int   DamageOverTimeTotalTicks;
		public static           bool  IsDealingDamageOverTime;
		public static           bool  IsAttacking;
		public static           bool  IsSwitching;
		private static readonly int   OcclusionStrength = Shader.PropertyToID("_OcclusionStrength");

		private float _moveSpeed; 
		private float _attackCooldownTime;
		private float _projectileSpeed;
		private float _abilityCooldownTime;
		private bool  _isReceivingDamage;
		
		public float MoveSmoothing               => moveSmoothing;
		public float MoveWhenAttackingMultiplier => currentPlayerElementalStats.moveWhenAttackingMultiplier;
		public float MoveBackwardsMultiplier     => moveBackwardsMultiplier;
		public float MoveSpeed                   => _moveSpeed;
		public float AttackCooldownTime          => _attackCooldownTime;
		public float ProjectileSpeed             => _projectileSpeed;
		public bool  HasAttacked                 { get; set; }

		public Animator Animator => _animator;

		private void Awake()
		{
			GetComponentReferences();
			SetBaseStats();
		}

		private void Start()
		{
			DeactivateAllAbilities();
			InitializeAbility(); 
		}

		private void Update()
		{
			if (ServiceLocator.Game.IsPaused)
				return;

			_move.Move(_input.MoveDirection);
			_aim.Aim(_input.AimDirection);

			IsAttacking = _input.FireInput;

			if (IsAttacking)
				ExecuteAbility();
			else
				StopAbility();
		}

#region Methods
		
		private void GetComponentReferences()
		{
			_input       = GetComponent<PlayerInput>();
			_aim         = GetComponent<PlayerAim>();
			_move        = GetComponent<PlayerMove>();
			_shoot       = GetComponent<PlayerShoot>();
			_elementType = GetComponent<ElementalSystemTypeCurrent>();
			_animator = GetComponentInChildren<Animator>();
			_collider = GetComponent<Collider>();
			_ability     = abilities[(int) ElementalSystemTypes.Base].GetComponent<IAbility>();
		}
		
		private void SetBaseStats()
		{
			PlayerDamageOverTime                = baseSettings.attackStrength * baseSettings.damageOverTimeMultiplier;
			DamageOverTimeCooldownTime          = baseSettings.damageOverTimeCooldownTime;
			DamageOverTimeTotalTicks            = baseSettings.damageOverTimeTotalTicks;
			_projectileSpeed                    = baseSettings.projectileSpeed;
			_abilityCooldownTime                = baseSettings.AbilityCooldownTime;
			GetComponent<Health>().MaxHitPoints = baseSettings.maxHitPoints;
			IsAttacking                         = false;
			SetElementBasedStats();
		}
		
		private void SetElementBasedStats()
		{
			PlayerDamage            = baseSettings.attackStrength * currentPlayerElementalStats.attackStrengthMultiplier;
			_moveSpeed              = baseSettings.moveSpeed      * currentPlayerElementalStats.moveSpeedMultiplier;
			_attackCooldownTime     = baseSettings.attackCooldownTime / currentPlayerElementalStats.attackRateMultiplier;
			IsDealingDamageOverTime = currentPlayerElementalStats.isDealingDamageOverTime;
		}
		
		public void SwitchElement()
		{
			SwitchElementalStats();
			SetElementBasedStats();
			SwitchAbility();
			SetAnimations();
			StartCoroutine(SwitchMaterial());
			ServiceLocator.HUD.UpdateElementBar(_elementType.Type, 0f);
		}

		private void SwitchElementalStats()
		{
			currentPlayerElementalStats = _elementType.Type switch {
				ElementalSystemTypes.Fire  => elementalStats[(int)ElementalSystemTypes.Fire],
				ElementalSystemTypes.Water => elementalStats[(int)ElementalSystemTypes.Water],
				ElementalSystemTypes.Earth => elementalStats[(int)ElementalSystemTypes.Earth],
				ElementalSystemTypes.Wind  => elementalStats[(int)ElementalSystemTypes.Wind],
				ElementalSystemTypes.Base  => elementalStats[(int)ElementalSystemTypes.Base],
				_                          => throw new ArgumentOutOfRangeException(nameof(_elementType.Type), _elementType.Type, null)
			};
		}

		private void SwitchAbility()
		{
			_ability.gameObject.SetActive(false);
			_ability = _elementType.Type switch {
				ElementalSystemTypes.Fire  => abilities[(int)ElementalSystemTypes.Fire].GetComponent<IAbility>(),
				ElementalSystemTypes.Water => abilities[(int)ElementalSystemTypes.Water].GetComponent<IAbility>(),
				ElementalSystemTypes.Earth => abilities[(int)ElementalSystemTypes.Earth].GetComponent<IAbility>(),
				ElementalSystemTypes.Wind  => abilities[(int)ElementalSystemTypes.Wind].GetComponent<IAbility>(),
				ElementalSystemTypes.Base  => abilities[(int)ElementalSystemTypes.Base].GetComponent<IAbility>(),
				_                          => throw new ArgumentOutOfRangeException(nameof(_elementType.Type), _elementType.Type, null)
			};
			InitializeAbility();
		}

		private void SetAnimations()
		{
			_animator.SetBool("IsEarthAttack", false);
			_animator.SetBool("IsSprayAttack", false);
			_animator.SetBool("IsWindAttack",  false);

			switch (_elementType.Type)
			{
				case ElementalSystemTypes.Base:
					return;
				case ElementalSystemTypes.Wind:
					_animator.SetBool("IsWindAttack", true);
					return;
				case ElementalSystemTypes.Earth:
					_animator.SetBool("IsEarthAttack", true);
					return;
				default:
					_animator.SetBool("IsSprayAttack", true);
					break;
			}
		}
		
		private IEnumerator SwitchMaterial()
		{
			IsSwitching = true;
			switchFX.SetActive(true);
			yield return new WaitForSeconds(0.3f);
			meshRenderer.material = currentPlayerElementalStats.material;
			yield return new WaitForSeconds(0.4f);
			switchFX.SetActive(false);
			IsSwitching = false;
		}
		
		private void DeactivateAllAbilities()
        {
        	foreach (GameObject ability in abilities)
        		ability.SetActive(false);
        }

		private void InitializeAbility()
		{
			_ability.gameObject.SetActive(true);
			_ability.Initialize(tag, PlayerDamage * baseSettings.abilityDamageMultiplier, _collider);
		}

		private void ExecuteAbility()
		{
			if (currentPlayerElementalStats != elementalStats[(int) ElementalSystemTypes.Base])
			{
				if (_ability.DidExecute())
					_animator.SetTrigger("DoAttack");
				
				_animator.SetBool("IsAttacking", true);
				return;
			}

			if (!_shoot.DidFire())
				return;
			
			_animator.SetTrigger("DoAttack");
			_animator.SetBool("IsAttacking", true);
		}
		
		private void StopAbility()
		{
			_ability.Stop();
			_animator.SetBool("IsAttacking", false);
		}

		public void UpdateHealthBar(float remainingPercent) => ServiceLocator.HUD.HealthBar.UpdateHealthBar(remainingPercent);

		public void ReceiveDamageEffect()
		{
			if (_isReceivingDamage)
				return;
			
			StartCoroutine(DamageEffectRoutine());
		}

		private IEnumerator DamageEffectRoutine()
		{
			float time          = 0;
			float fadeDuration  = 0.25f;
			float originalValue = meshRenderer.materials[0].GetFloat(OcclusionStrength);
			float newValue      = 120f;
			
			_isReceivingDamage = true;
			meshRenderer.materials[0].SetFloat(OcclusionStrength, newValue);

			while (time < fadeDuration) {
				float valueToSet = Mathf.Lerp(newValue, originalValue, time / fadeDuration);
				meshRenderer.materials[0].SetFloat(OcclusionStrength, valueToSet);
				time += Time.deltaTime;
				yield return null;
			}
			
			meshRenderer.materials[0].SetFloat(OcclusionStrength, originalValue);
			_isReceivingDamage = false;
		}

		public void UpdateStatsFromEditorWindow()
		{
			SetBaseStats();
			SetElementBasedStats();
		}
		
		#endregion

	}
}