using _Project.Scripts.ElementalSystem;
using UnityEngine;

namespace _Project.Scripts.Enemies.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyElementalStats", menuName = "EnemyElementalStats", order = 0)]
    public class EnemyElementalStats : ScriptableObject
    {
        // [Header("ELEMENTAL BASE:")]
        public ElementalSystemTypes elementType;
        public GameObject ability;
        public float maxHitPoints             = 200f;
        
        // [Header("ELEMENTAL MODIFIERS:")]
        public float moveSpeedMultiplier      = 1f;
        public float attackStrengthMultiplier = 1f;
        public float attackRateMultiplier     = 1f;
        public float attackRange = 5f;
        public bool  hasRangedAttack          = false;

        public GameObject mesh;
        public RuntimeAnimatorController animatorController;
        public Vector3 abilityOffset;
    }
}