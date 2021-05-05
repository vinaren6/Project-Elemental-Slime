using UnityEngine;

namespace _Project.Scripts.Player.ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewPlayerElementalStats", menuName = "PlayerElementalStats", order = 0)]
	public class PlayerElementalStats : ScriptableObject
	{
		// [Header("ELEMENTAL MODIFIERS:")]
		public float      moveSpeedMultiplier      = 1f;
		public float      damageReceivedMultiplier = 1f;
		public float      attackStrengthMultiplier = 1f;
		public float      attackRateMultiplier     = 1f;
		public bool       isDealingDamageOverTime  = false;
	}
}