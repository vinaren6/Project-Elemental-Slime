using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class PlayerHUD : MonoBehaviour, IHUD
    {
        [SerializeField] private HealthUI       healthBar;
        [SerializeField] private ElementBarUI[] elementBars;

        public HealthUI HealthBar => healthBar;
        
        private void Start()
        {
            ServiceLocator.ProvideHUD(this);
        }
        
        public void UpdateElementBar(ElementalSystemTypes type, float percent)
        {
            if (type == ElementalSystemTypes.Base)
                return;
            
            elementBars[(int)type].UpdateUI(percent);
        }

        public ElementBarUI ElementBar(ElementalSystemTypes type)
        {
            return elementBars[(int) type];
        }
    }
}