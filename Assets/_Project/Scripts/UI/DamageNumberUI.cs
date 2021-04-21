using System;
using _Project.Scripts.HealthSystem;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class DamageNumberUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private Color[] colors;
        
        private Transform _transform;
        private Animator _animator;
        
        private void Awake()
        {
            Health.onAnyReceiveDamage += ShowDamage;
            _transform                =  transform;
            _animator                 =  GetComponent<Animator>();
        }

        private void ShowDamage(Vector3 position, int damage, float elementalMultiplier)
        {
            numberText.text    = damage.ToString();
            _transform.position = position;
            numberText.color   = GetEffectiveColor(elementalMultiplier);
            _animator.SetTrigger("Execute");
        }

        private Color GetEffectiveColor(float elementalMultiplier)
        {
            if (elementalMultiplier < 1)
                return colors[(int) EffectiveType.Weakness];
            if (elementalMultiplier > 1)
                return colors[(int) EffectiveType.Effective];
            return colors[(int) EffectiveType.Neutral];
        }

        public void AnimationEnd()
        {
            
        }
    }
}
