using System.Collections;
using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public class ElementalSystemColorShift : MonoBehaviour
	{
		[SerializeField] private ElementalSystemTypeCurrent type;

		private Renderer _renderer;
		private Color    _targetColor;

		private void Start() => _renderer = gameObject.GetComponent<Renderer>();

		public void StartColorShift()
		{
			_targetColor = ElementalSystemColors.GetColorFromElementalSystemTypes(type.Type);
			StartCoroutine(UpdateColor());
		}

		private IEnumerator UpdateColor()
		{
			while (_renderer.material.color != _targetColor) {
				_renderer.material.color = Color.Lerp(
					_renderer.material.color, _targetColor, Time.deltaTime * 0.5f);
				yield return null;
			}
		}
	}
}