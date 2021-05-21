using _Project.Scripts.ElementalSystem;

namespace _Project.Scripts.UI
{
    public interface IHUD
    {
        public HealthUI HealthBar { get; }
        public ElementBarUI ElementBar(ElementalSystemTypes type);
        public void UpdateElementBar(ElementalSystemTypes type, float percent);
    }
}