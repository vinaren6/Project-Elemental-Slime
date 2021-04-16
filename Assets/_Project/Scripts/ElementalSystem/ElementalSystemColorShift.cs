using System.Collections;
using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public class ElementalSystemColorShift : MonoBehaviour
	{
		[SerializeField] private ElementalSystemTypeCurrent type;
		[SerializeField] private float                      fadeSpeed = .5f;
		[SerializeField] private bool                       startColorShiftOnStart;

		private Renderer _renderer;
		private Color    _targetColor;

		private void Start()
		{
			if (startColorShiftOnStart) StartColorShift();
		}

		private void OnEnable() => _renderer ??= gameObject.GetComponent<Renderer>();

		public void StartColorShift()
		{
			_targetColor = ElementalSystemColors.GetColorFromElementalSystemTypes(type.Type);
			StartCoroutine(UpdateColor());
		}

		public void SetColor() => _renderer.material.color =
			_targetColor = ElementalSystemColors.GetColorFromElementalSystemTypes(type.Type);

		private IEnumerator UpdateColor()
		{
			while (_renderer.material.color != _targetColor) {
				_renderer.material.color = Color.Lerp(
					_renderer.material.color, _targetColor, Time.deltaTime * fadeSpeed);
				yield return null;
			}
		}
	}
}