using System;
using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public static class ElementalSystemColors
	{
		public static Color[] colors;

		//set colors to default values
		static ElementalSystemColors() =>
			colors = new Color[] {
				new Color32(236, 190, 135, 255),
				new Color32(120, 238, 169, 255),
				new Color32(36,  199, 242, 255),
				new Color32(235, 70,  75,  255),
				Color.white
			};

		public static Color GetColorFromElementalSystemTypes(ElementalSystemTypes type) =>
			type switch {
				ElementalSystemTypes.Earth => colors[0],
				ElementalSystemTypes.Wind  => colors[1],
				ElementalSystemTypes.Water => colors[2],
				ElementalSystemTypes.Fire  => colors[3],
				ElementalSystemTypes.Base  => colors[4],
				_                          => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
	}
}