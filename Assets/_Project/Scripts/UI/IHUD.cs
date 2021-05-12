using _Project.Scripts.ElementalSystem;

namespace _Project.Scripts.UI
{
    public interface IHUD
    {
        public HealthUI Healthbar { get; }
        public ElementBarUI ElementBar(ElementalSystemTypes type);
        public void UpdateElementBar(ElementalSystemTypes type, float percent);
        public SpecialAttackUIOLD SpecialAttack { get; }
    }
}