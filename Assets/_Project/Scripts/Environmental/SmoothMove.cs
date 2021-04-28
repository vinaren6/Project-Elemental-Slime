using UnityEngine;

namespace _Project.Scripts.Environmental
{
	public class SmoothMove : MonoBehaviour
	{
		[SerializeField] private float   smoothTime = 1f;
		private                  Vector3 _t;
		private                  Vector3 _targetPos;

		private void Update()
		{
			if (_targetPos == transform.position) {
				enabled = false;
				return;
			}

			transform.position = Vector3.SmoothDamp(transform.position, _targetPos, ref _t, smoothTime);
		}

		public void GoToo(Vector3 target)
		{
			_targetPos = target;
			enabled    = true;
		}

		public void GoTooX(float target) => GoToo(new Vector3(target, transform.position.y, transform.position.z));
		public void GoTooY(float target) => GoToo(new Vector3(transform.position.x, target, transform.position.z));
		public void GoTooZ(float target) => GoToo(new Vector3(transform.position.x, transform.position.y, target));
	}
}