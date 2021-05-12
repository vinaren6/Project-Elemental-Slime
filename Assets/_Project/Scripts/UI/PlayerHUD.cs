using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class PlayerHUD : MonoBehaviour, IHUD
    {
        [SerializeField] private HealthUI _healthbar;
        [SerializeField] private SpecialAttackUIOLD _specialAttack;
        [SerializeField] private ElementBarUI[] _elementBars;

        public HealthUI Healthbar => _healthbar;
        
        public void UpdateElementBar(ElementalSystemTypes type, float percent)
        {
            if (type == ElementalSystemTypes.Base)
                return;
            
            _elementBars[(int)type].UpdateUI(percent);
        }

        public SpecialAttackUIOLD SpecialAttack => _specialAttack;

        private void Start()
        {
            ServiceLocator.ProvideHUD(this);
        }
        
        public ElementBarUI ElementBar(ElementalSystemTypes type)
        {
            return _elementBars[(int) type];
        }
    }
}