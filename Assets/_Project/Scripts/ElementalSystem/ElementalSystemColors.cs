using System;
using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public static class ElementalSystemColors
	{
		public static Color GetColorFromElementalSystemTypes(ElementalSystemTypes type) =>
			type switch {
				ElementalSystemTypes.Earth => new Color32(236, 190, 135, 255),
				ElementalSystemTypes.Wind  => new Color32(120, 238, 169, 255),
				ElementalSystemTypes.Water => new Color32(36,  199, 242, 255),
				ElementalSystemTypes.Fire  => new Color32(235, 70,  75,  255),
				ElementalSystemTypes.Base  => Color.white,
				_                          => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
	}
}