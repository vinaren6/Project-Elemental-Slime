using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public interface IAbility
    {
        GameObject gameObject { get; }
        
        void Initialize(float damage);
        
        void Execute();
    }
}