using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	[CreateAssetMenu(fileName = "ColorProfile", menuName = "Color Profile", order = 0)]
	public class SOElementalSystemColorProfile : ScriptableObject
	{
		public Color EarthColor;
		public Color WindColor;
		public Color WaterColor;
		public Color FireColor;
		public Color BaseColor;
	}
}