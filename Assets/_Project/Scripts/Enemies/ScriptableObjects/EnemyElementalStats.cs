using UnityEngine;

namespace _Project.Scripts.Enemies.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyElementalStats", menuName = "EnemyElementalStats", order = 0)]
    public class EnemyElementalStats : ScriptableObject
    {
        [Header("ELEMENTAL MODIFIERS:")]
        public float maxHitPoints             = 200f;
        public float moveSpeedMultiplier      = 1f;
        public float attackStrengthMultiplier = 1f;
        public float attackRateMultiplier     = 1f;
        public bool  hasRangedAttack          = false;
		
        public GameObject specialAttack;
    }
}