using _Project.Scripts.Managers;
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

		private void Awake()
		{
			_transform        = transform;
			_animator         = GetComponent<Animator>();
			killText.font     = inGameUI.inGameFont;
			killText.fontSize = inGameUI.killTextFontSize;
			killText.color    = inGameUI.killTextColor;
		}

		public void ShowKillText(Vector3 position)
		{
			_transform.position = position;
			_animator.SetTrigger("Execute");
		}
        
		public void AnimationEnd()
		{
			ServiceLocator.Pools.ReturnObjectToPool(PoolType.KillText, gameObject);
		}
	}
}