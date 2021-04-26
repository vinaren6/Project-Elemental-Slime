using System.Collections;
using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public class ElementalSystemColorShift : MonoBehaviour
	{
		[SerializeField] private ElementalSystemTypeCurrent type;
		[SerializeField] private float                      fadeSpeed = .5f;
		[SerializeField] private bool                       startColorShiftOnStart;
		[SerializeField] private Renderer                   meshRenderer;

		private Color _targetColor;

		private void Start()
		{
			if (startColorShiftOnStart) StartColorShift();
		}

		public void StartColorShift()
		{
			_targetColor = ElementalSystemColors.GetColorFromElementalSystemTypes(type.Type);
			StartCoroutine(UpdateColor());
		}

		public void SetColor() => meshRenderer.material.color =
			_targetColor = ElementalSystemColors.GetColorFromElementalSystemTypes(type.Type);

		public void ResetColor() => meshRenderer.material.color =
			_targetColor = Color.white;

		private IEnumerator UpdateColor()
		{
			while (meshRenderer.material.color != _targetColor) {
				meshRenderer.material.color = Color.Lerp(
					meshRenderer.material.color, _targetColor, Time.deltaTime * fadeSpeed);
				yield return null;
			}
		}
	}
}