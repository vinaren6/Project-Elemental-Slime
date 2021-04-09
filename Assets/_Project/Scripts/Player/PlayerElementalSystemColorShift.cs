using System.Collections;
using _Project.Scripts.ElementalSystem;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class PlayerElementalSystemColorShift : MonoBehaviour
	{
		[SerializeField] private PlayerElementalSystemTypeCurrent type;

		private Renderer _renderer;
		private void     Start() => _renderer = gameObject.GetComponent<Renderer>();

		public void StartColorShift() =>
			StartCoroutine(UpdateColor(ElementalSystemColors.GetColorFromElementalSystemTypes(type.Type)));

		private IEnumerator UpdateColor(Color targetColor)
		{
			while (_renderer.material.color != targetColor) {
				_renderer.material.color = Color.Lerp(
					_renderer.material.color, targetColor, 0.01f + Time.deltaTime);
				yield return null;
			}
		}
	}
}