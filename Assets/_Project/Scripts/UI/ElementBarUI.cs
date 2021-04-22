using _Project.Scripts.ElementalSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ElementBarUI : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image fillImage;
        [SerializeField] private GameObject highlight;
        [SerializeField] private ElementalSystemTypes type;
        [SerializeField] private Color[] elementColors;
        
        private void Awake()
        {
            fillImage.fillAmount = 0;
            highlight.SetActive(false);
            SetupColors();
        }

        public void UpdateUI(float percent)
        {
            fillImage.fillAmount = percent;

            highlight.SetActive(percent >= 1f);
        }

        [ContextMenu("SetupColors")]
        private void SetupColors()
        {
            backgroundImage.color = elementColors[(int) type * 2];
            fillImage.color = elementColors[(int) type * 2 + 1];
        }
    }
}
