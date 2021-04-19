using UnityEngine;

namespace _Project.Scripts.Player.ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewElementalPlayerStats", menuName = "ElementalPlayerStats", order = 0)]
	public class ElementalPlayerStats : ScriptableObject
	{
		[Header("ELEMENTAL MODIFIERS:")]
		public float moveSpeedMultiplier      = 1f;
		public float damageReceivedMultiplier = 1f;
		public float attackStrengthMultiplier = 1f;
		public float attackRateMultiplier     = 1f;
		public bool  isDealingDamageOverTime  = false;
	}
}