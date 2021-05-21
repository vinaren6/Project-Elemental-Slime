using _Project.Scripts.ElementalSystem;

namespace _Project.Scripts.UI
{
    public class NullHUD : IHUD
    {
        public HealthUI HealthBar { get; }
        
        public ElementBarUI ElementBar(ElementalSystemTypes type)
        {
            return null;
        }

        public void UpdateElementBar(ElementalSystemTypes type, float percent)
        {
        }
    }
}