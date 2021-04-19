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
			switch (baseType) {
				case ElementalSystemTypes.Earth when targetType == ElementalSystemTypes.Fire:
					return 2;
				case ElementalSystemTypes.Fire when targetType == ElementalSystemTypes.Earth:
					return .5f;
				default: {
					float mult = baseType == ElementalSystemTypes.Base || targetType == ElementalSystemTypes.Base
						? 1
						: 1 + ((int) baseType - (int) targetType) % 2;
					return mult == 0 ? .5f : mult;
				}
			}
		}
	}
}

// 0 - Earth,
// 1 - Wind,
// 2 - Water,
// 3 - Fire,
// 4 - Base