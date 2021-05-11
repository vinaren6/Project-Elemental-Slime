using System;
using _Project.Scripts.Managers;
using _Project.Scripts.UI.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class DamageNumberUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text numberText;
        [SerializeField] private InGameUI inGameUI;

        private Transform _transform;
        private Animator _animator;
        
        private void Awake()
        {
            _transform                =  transform;
            _animator                 =  GetComponent<Animator>();
        }

        public void ShowDamage(Vector3 position, int damage, EffectiveType colorType)
        {
            numberText.text     = damage.ToString();
            _transform.position = position;
            numberText.fontSize = inGameUI.GetDamageNumberFontSize(colorType.ToString());
            numberText.color    = inGameUI.GetDamageNumberColor(colorType.ToString());
            _animator.SetTrigger("Execute");
        }
        
        public void AnimationEnd()
        {
            ServiceLocator.DamageNumbers.ReturnNumberToPool(gameObject);
        }
    }
}
