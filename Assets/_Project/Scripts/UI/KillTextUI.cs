using UnityEngine;

namespace _Project.Scripts.UI
{
	public class KillTextUI : MonoBehaviour
	{
		private Transform        _transform;
		private Animator         _animator;
		private KillFeedbackPool _killFeedbackPool;
        
		private void Awake()
		{
			_transform        = transform;
			_animator         = GetComponent<Animator>();
			_killFeedbackPool = GetComponentInParent<KillFeedbackPool>();
		}

		public void ShowKillText(Vector3 position)
		{
			_transform.position = position;
			_animator.SetTrigger("Execute");
		}
        
		public void AnimationEnd()
		{
			_killFeedbackPool.ReturnNumberToPool(gameObject);
		}
	}
}