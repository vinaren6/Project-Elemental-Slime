using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public interface IAbility
    {
        GameObject gameObject { get; }
        
        void Initialize(string newTag, float damage, Collider selfCollider = null);
        void Initialize(string newTag, float damage, float distance, Collider selfCollider = null);

        bool DidExecute();

        void Stop(bool isPlayer = true);

        bool IsInRange();

        bool IsInWalkRange();
        bool CanAttack();
        float GetAttackTime();
        bool StopLooping { get; }
    }
}