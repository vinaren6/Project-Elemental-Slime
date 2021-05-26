using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public static class ElementalSystemMultiplier
	{
		/*
		public static float GetMultiplier(ElementalSystemTypes baseType, ElementalSystemTypes targetType) =>
			baseType == ElementalSystemTypes.Base || targetType == ElementalSystemTypes.Base
				? 1
				: 1 + ((int) baseType - (int) targetType) % 2;
				*/
		public static float GetMultiplier(ElementalSystemTypes baseType, ElementalSystemTypes targetType)
		{
			return targetType switch
			{
				ElementalSystemTypes.Fire when baseType == ElementalSystemTypes.Earth => 2f,
				ElementalSystemTypes.Fire when baseType == ElementalSystemTypes.Water => 0.5f,
				ElementalSystemTypes.Fire => 1f,
				ElementalSystemTypes.Water when baseType == ElementalSystemTypes.Fire => 2f,
				ElementalSystemTypes.Water when baseType == ElementalSystemTypes.Wind => 0.5f,
				ElementalSystemTypes.Water => 1f,
				ElementalSystemTypes.Earth when baseType == ElementalSystemTypes.Wind => 2f,
				ElementalSystemTypes.Earth when baseType == ElementalSystemTypes.Fire => 0.5f,
				ElementalSystemTypes.Earth => 1f,
				ElementalSystemTypes.Wind when baseType == ElementalSystemTypes.Water => 2f,
				ElementalSystemTypes.Wind when baseType == ElementalSystemTypes.Earth => 0.5f,
				ElementalSystemTypes.Wind => 1f,
				_ => 1f
			};

			// switch (baseType) {
			// 	case ElementalSystemTypes.Fire when targetType == ElementalSystemTypes.Wind:
			// 		return 2;
			// 	case ElementalSystemTypes.Wind when targetType == ElementalSystemTypes.Fire:
			// 		return .5f;
			// 	default: {
			// 		float mult = baseType == ElementalSystemTypes.Base || targetType == ElementalSystemTypes.Base
			// 			? 1
			// 			: 1 + ((int) baseType - (int) targetType) % 2;
			// 		return mult == 0 ? .5f : mult;
			// 	}
			// }
		}
	}
}

// 0 - Fire,
// 1 - Water,
// 2 - Earth,
// 3 - Wind,
// 4 - Base

// baseType = the characterType that gets hit
// targetType = the attackType that deals damage