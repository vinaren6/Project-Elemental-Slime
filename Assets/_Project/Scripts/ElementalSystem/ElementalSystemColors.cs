using System;
using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public static class ElementalSystemColors
	{
		public static Color GetColorFromElementalSystemTypes(ElementalSystemTypes type) =>
			type switch {
				ElementalSystemTypes.Earth => Color.green,
				ElementalSystemTypes.Wind  => Color.yellow,
				ElementalSystemTypes.Water => Color.blue,
				ElementalSystemTypes.Fire  => Color.red,
				ElementalSystemTypes.Base  => Color.white,
				_                          => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
	}
}