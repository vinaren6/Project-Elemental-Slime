using _Project.Scripts.ElementalSystem;

namespace _Project.Scripts.UI
{
    public class NullHUD : IHUD
    {
        public HealthUI Healthbar { get; }
        public ElementBarUI ElementBar(ElementalSystemTypes type)
        {
            return null;
        }

        public void UpdateElementBar(ElementalSystemTypes type, float percent)
        {
        }

        public SpecialAttackUIOLD SpecialAttack { get; }
    }
}