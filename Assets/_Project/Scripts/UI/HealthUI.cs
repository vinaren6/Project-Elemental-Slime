using System;
using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class HealthUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        private void Awake() => fillImage.fillAmount      =  1f;

        public void UpdateHealthBar(float percent) => fillImage.fillAmount = percent;
    }
}