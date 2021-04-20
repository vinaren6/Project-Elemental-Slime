using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class SpecialAttackUI : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private GameObject highlight;

        private void Awake()
        {
            fillImage.fillAmount = 0f;
            highlight.SetActive(false);
            UpdateUI(1f);
        }

        public void StartCooldown(float cooldownTime)
        {
            StartCoroutine(CooldownRoutine(cooldownTime));
        }

        private IEnumerator CooldownRoutine(float cooldownTime)
        {
            float time = 0f;

            while (time < cooldownTime)
            {
                time += Time.deltaTime;
                
                UpdateUI(time / cooldownTime);

                yield return null;
            }
        }

        public void UpdateUI(float percent)
        {
            fillImage.fillAmount = percent;
            
            highlight.SetActive(percent >= 1f);
        }
    }
}
