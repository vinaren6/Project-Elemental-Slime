namespace _Project.Scripts.Abilities
{
    public interface IAbility
    {
        void Initialize(float damage);
        
        void Execute();
    }
}