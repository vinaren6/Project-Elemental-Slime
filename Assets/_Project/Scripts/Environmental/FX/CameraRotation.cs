using UnityEngine;

namespace _Project.Scripts.Environmental.FX
{
	public class CameraRotation : MonoBehaviour
	{
		[SerializeField] private float      rotationSpeed = 1f;
		private                  Quaternion _baseRoot;
		private                  float      _t;
		private                  void       Awake() => _baseRoot = transform.rotation;

		private void Update()
		{
			if (Time.timeScale == 0)
				transform.rotation = Quaternion.Euler(
					_baseRoot.eulerAngles + new Vector3(
						(Mathf.Cos((_t += Time.fixedUnscaledDeltaTime) * rotationSpeed * 0.05f) - 1) * 10f,
						_t * rotationSpeed, 0));
			else {
				transform.rotation = _baseRoot;
				_t                 = 0;
			}
		}
	}
}