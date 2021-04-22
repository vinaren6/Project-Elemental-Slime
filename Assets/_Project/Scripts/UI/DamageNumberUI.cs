using _Project.Scripts.Managers;
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
            _transform                =  transform;
            _animator                 =  GetComponent<Animator>();
        }

        public void ShowDamage(Vector3 position, int damage, EffectiveType colorType)
        {
            numberText.text    = damage.ToString();
            _transform.position = position;
            // numberText.color   = GetEffectiveColor(elementalMultiplier);
            numberText.color = colors[(int) colorType];
            _animator.SetTrigger("Execute");
        }
        
        public void AnimationEnd()
        {
            ServiceLocator.DamageNumbers.ReturnNumberToPool(gameObject);
        }
        
        // private Color GetEffectiveColor(float elementalMultiplier)
        // {
        //     if (elementalMultiplier < 1)
        //         return colors[(int) EffectiveType.Weakness];
        //     if (elementalMultiplier > 1)
        //         return colors[(int) EffectiveType.Effective];
        //     return colors[(int) EffectiveType.Neutral];
        // }
    }
}
