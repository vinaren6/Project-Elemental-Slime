using _Project.Scripts.Managers;
using _Project.Scripts.UI.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class ComboTextUI : MonoBehaviour
	{
		[SerializeField] private InGameUI inGameUI;
		[SerializeField] private TMP_Text comboText;
	
		private Transform        _transform;
		private Animator         _animator;

		private void Awake()
		{
			_transform         = transform;
			_animator          = GetComponent<Animator>();
			comboText.font     = inGameUI.inGameFont;
			comboText.fontSize = inGameUI.comboTextFontSize;
			comboText.color    = inGameUI.comboTextColor;
		}

		public void ShowComboText(Vector3 position)
		{
			_transform.position = position;
			_animator.SetTrigger("Execute");
		}
    
		public void AnimationEnd()
		{
			ServiceLocator.Pools.ReturnObjectToPool(PoolType.ComboText, gameObject);
		}
	}
}