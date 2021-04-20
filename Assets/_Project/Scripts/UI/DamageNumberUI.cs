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
            _transform = transform;
            _animator = GetComponent<Animator>();
        }
        
        public void ShowDamage(Vector3 position, int damage, EffectiveType type)
        {
            _transform.position = position;
            numberText.color = colors[(int) type];
            numberText.text = damage.ToString();
            _animator.SetTrigger("Execute");
        }

        public void AnimationEnd()
        {
            
        }
    }
}
