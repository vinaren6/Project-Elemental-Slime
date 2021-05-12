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
		private KillFeedbackPool _killFeedbackPool;

		private void Awake()
		{
			_transform         = transform;
			_animator          = GetComponent<Animator>();
			_killFeedbackPool  = GetComponentInParent<KillFeedbackPool>();
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
			_killFeedbackPool.ReturnComboTextToPool(gameObject);
		}
	}
}