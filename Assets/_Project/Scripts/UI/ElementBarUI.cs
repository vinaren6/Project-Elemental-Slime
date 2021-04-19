using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class ElementBarUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private Image highlightImage;

        private void Awake()
        {
            highlightImage.enabled = false;
        }

        public void UpdateElementPercent(float percent)
        {
            fillImage.fillAmount = percent;

            highlightImage.enabled = percent >= 1f;
        }
    }

}
