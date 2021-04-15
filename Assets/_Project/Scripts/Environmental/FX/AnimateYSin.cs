using UnityEngine;

namespace _Project.Scripts.Environmental.FX
{
	public class AnimateYSin : MonoBehaviour
	{
		[SerializeField] private float strenght = 1;
		[Range(0, 10f)]
		[SerializeField] private float speed = 1;

		private void FixedUpdate()
		{
			transform.localPosition = new Vector3(
				transform.localPosition.x, 
				Mathf.Sin(Time.fixedTime * speed) * strenght, 
				transform.localPosition.z);
		}
	}
}