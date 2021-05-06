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
			Vector3 localPosition;
			if (_targetPos == (localPosition = transform.localPosition)) {
				enabled = false;
				return;
			}

			transform.localPosition = Vector3.SmoothDamp(localPosition, _targetPos, ref _t, smoothTime);
		}

		public void GoToo(Vector3 target)
		{
			_targetPos = target;
			enabled    = true;
		}

		public void GoTooX(float target)
		{
			Vector3 localPosition;
			GoToo(new Vector3(target, (localPosition = transform.localPosition).y, localPosition.z));
		}

		public void GoTooY(float target)
		{
			Transform transform1;
			GoToo(new Vector3((transform1 = transform).localPosition.x, target, transform1.localPosition.z));
		}

		public void GoTooZ(float target)
		{
			Vector3 localPosition;
			GoToo(new Vector3((localPosition = transform.localPosition).x, localPosition.y, target));
		}
	}
}