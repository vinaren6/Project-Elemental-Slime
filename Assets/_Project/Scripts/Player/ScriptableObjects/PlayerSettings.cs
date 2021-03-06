using UnityEngine;

namespace _Project.Scripts.Player.ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewPlayerSettings", menuName = "PlayerSettings", order = 0)]
	public class PlayerSettings : ScriptableObject
	{
		// [Header("PLAYER BASE")]
		public float maxHitPoints               = 125f;
		
		// [Header("PLAYER MOVEMENT:")] 
		public float moveSpeed                  = 6f;
		
		// [Header("Attack:")] 
		public float attackStrength             = 1f;
		public float attackCooldownTime         = 0.5f;
		public float projectileSpeed            = 0.06f;
		public float damageOverTimeCooldownTime = 0.5f;
		public float damageOverTimeMultiplier   = 0.2f;
		public int   damageOverTimeTotalTicks   = 4;
		public float abilityDamageMultiplier    = 5f;
		public float AbilityCooldownTime        = 3f;
	}
}