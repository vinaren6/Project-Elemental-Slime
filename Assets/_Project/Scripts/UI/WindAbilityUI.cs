using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class WindAbilityUI : MonoBehaviour
    {
        private Image[] _chargeImages;

        private Transform _transform;
        
        private void Awake()
        {
            GetAllComponents();
        }

        private void GetAllComponents()
        {
            _transform = transform;
            
            _chargeImages = new Image[_transform.childCount];
            for (int i = 0; i < _chargeImages.Length; i++)
            {
                _chargeImages[i] = _transform.GetChild(i).GetChild(0).GetComponent<Image>();
            }
        }

        public void UpdateUI(float chargesAmount)
        {
            for (int i = _chargeImages.Length - 1; i >= 0; i--)
            {
                _chargeImages[i].fillAmount = Mathf.Clamp01(chargesAmount);

                chargesAmount--;
            }
        }
    }
}
