using UnityEngine;

namespace _Project.Scripts.Player.ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewPlayerSettings", menuName = "PlayerSettings", order = 0)]
	public class PlayerSettings : ScriptableObject
	{
		[Header("UNIVERSAL PLAYER SETTINGS:")]
		public float moveSpeed                = 6f;
		public float damageReceived           = 1f;
		public float attackStrength           = 1f;
		public float attackRate               = 0.5f;
		public float projectileSpeed          = 0.06f;
		public float damageOverTimeRate       = 0.5f;
		public float damageOverTimeMultiplier = 0.2f;
		public float specialAttackRate        = 3f;
	}
}