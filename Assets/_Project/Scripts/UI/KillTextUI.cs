using _Project.Scripts.UI.ScriptableObjects;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class KillTextUI : MonoBehaviour
	{
		[SerializeField] private InGameUI inGameUI;
		[SerializeField] private TMP_Text killText;
		
		private                  Transform        _transform;
		private                  Animator         _animator;
		private                  KillFeedbackPool _killFeedbackPool;

		private void Awake()
		{
			_transform        = transform;
			_animator         = GetComponent<Animator>();
			_killFeedbackPool = GetComponentInParent<KillFeedbackPool>();
			killText.font     = inGameUI.inGameFont;
			killText.fontSize = inGameUI.killTextFontSize;
		}

		public void ShowKillText(Vector3 position)
		{
			_transform.position = position;
			_animator.SetTrigger("Execute");
		}
        
		public void AnimationEnd()
		{
			_killFeedbackPool.ReturnKillTextToPool(gameObject);
		}
	}
}