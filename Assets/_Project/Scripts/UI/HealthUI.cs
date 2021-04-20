using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        public void UpdateHealthBar(float percent)
        {
            fillImage.fillAmount = percent;
        }
    }
}