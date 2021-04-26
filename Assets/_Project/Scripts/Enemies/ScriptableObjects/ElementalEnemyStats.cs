using UnityEngine;

namespace _Project.Scripts.Enemies.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewElementalEnemyStats", menuName = "ElementalEnemyStats", order = 0)]
    public class ElementalEnemyStats : ScriptableObject
    {
        [Header("ELEMENTAL MODIFIERS:")]
        public float      moveSpeedMultiplier      = 1f;
        public float      damageReceivedMultiplier = 1f;
        public float      attackStrengthMultiplier = 1f;
        public float      attackRateMultiplier     = 1f;
        public bool       hasRangedAttack  = false;
		
        public GameObject specialAttack;
    }
}