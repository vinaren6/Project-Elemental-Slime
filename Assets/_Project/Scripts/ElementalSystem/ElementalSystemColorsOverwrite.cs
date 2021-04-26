using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public class ElementalSystemColorsOverwrite : MonoBehaviour
	{
		[SerializeField] private SOElementalSystemColorProfile colorProfile;

		private void Awake() => ElementalSystemColors.colors = new[] {
			colorProfile.EarthColor, colorProfile.WindColor, colorProfile.WaterColor, colorProfile.FireColor,
			colorProfile.BaseColor
		};
	}
}