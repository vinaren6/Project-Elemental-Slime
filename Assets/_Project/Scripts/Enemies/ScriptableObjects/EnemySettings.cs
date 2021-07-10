using UnityEngine;

namespace _Project.Scripts.Enemies.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemySettings", menuName = "EnemySettings", order = 0)]
    public class EnemySettings : ScriptableObject
    {
        // [Header("ENEMY SETTINGS:")]
        public float moveSpeed          = 6f;
        public float attackStrength     = 1f;
        public float attackCooldownTime = 0.5f;
        [Space(4)] 
        public bool  detectAnimation    = true;
    }
}