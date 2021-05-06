using UnityEngine;

namespace _Project.Scripts.Environmental.FX
{
	public class AnimateYSin : MonoBehaviour
	{
		[SerializeField] private float strength = 1;

		[Range(0, 10f)] [SerializeField] private float speed = 1;

		private float _offset;
		private void  Start() => _offset = Time.fixedTime % 1;

		private void FixedUpdate()
		{
			Vector3 localPosition = transform.localPosition;
			localPosition = new Vector3(
				localPosition.x,
				Mathf.Sin(Time.fixedTime * speed + _offset) * strength,
				localPosition.z);
			transform.localPosition = localPosition;
		}
	}
}