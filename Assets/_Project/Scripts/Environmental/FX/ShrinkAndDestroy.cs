using UnityEngine;

namespace _Project.Scripts.Environmental.FX
{
	public class ShrinkAndDestroy : MonoBehaviour
	{
		[SerializeField] private Transform target;
		[SerializeField] private float     time = 5;
		private                  float     _t;
		private                  Vector3   _baseScale;

		private void Start()
		{
			_baseScale        = target.localScale;
			target.localScale = Vector3.zero;
		}

		private void FixedUpdate()
		{
			_t += Time.fixedDeltaTime;
			if (_t >= time)
				Destroy(gameObject);
			else
				target.localScale = _baseScale * (1 - _t / time);
		}
	}
}