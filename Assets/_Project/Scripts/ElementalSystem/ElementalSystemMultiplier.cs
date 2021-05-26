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
				case ElementalSystemTypes.Fire when targetType == ElementalSystemTypes.Wind:
					return 2;
				case ElementalSystemTypes.Wind when targetType == ElementalSystemTypes.Fire:
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

// 0 - Fire,
// 1 - Water,
// 2 - Earth,
// 3 - Wind,
// 4 - Base