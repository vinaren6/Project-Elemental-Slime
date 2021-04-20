using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class DamageNumberUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberText;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void AnimationEnd()
        {
            
        }

        private void ShowDamage(int damage)
        {
            numberText.text = damage.ToString();
            _animator.SetTrigger("Execute");
        }

    }
}
