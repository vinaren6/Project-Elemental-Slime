using _Project.Scripts.ElementalSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ElementBarUI : MonoBehaviour
    {
        
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image fillImage;
        [SerializeField] private Image highlightImage;
        [SerializeField] private ElementalSystemTypes type;
        [SerializeField] private Color[] elementColors;
        
        private void Awake()
        {
            highlightImage.enabled = false;
            SetupColors();
        }

        public void UpdateElementPercent(float percent)
        {
            fillImage.fillAmount = percent;

            highlightImage.enabled = percent >= 1f;
        }

        private void SetupColors()
        {
            backgroundImage.color = elementColors[(int) type];
            // highlightImage.color = elementColors[(int) type * 2 + 1];
        }
    }
}
