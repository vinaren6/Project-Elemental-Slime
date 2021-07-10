using UnityEngine;

namespace _Project.Scripts.Environmental.FX
{
	public class ScaleInOnAwake : MonoBehaviour
	{
		[SerializeField] private float speed = 5;
		private                  float _t = 0;
		private void FixedUpdate()
		{
			_t += Time.fixedDeltaTime * speed;
			if (_t >= 1) {
				transform.localScale = new Vector3(1, 1, 1);
				enabled              = false;
			} else
				transform.localScale = new Vector3(_t, _t, _t);
		}
	}
}