namespace _Project.Scripts.ElementalSystem
{
	public static class ElementalSystemMultiplier
	{
		public static float GetMultiplier(ElementalSystemTypes baseType, ElementalSystemTypes targetType) =>
			baseType == ElementalSystemTypes.Base || targetType == ElementalSystemTypes.Base
				? 1
				: 1 + ((int) baseType - (int) targetType) % 2;
	}
}

// 0 - Earth,
// 1 - Wind,
// 2 - Water,
// 3 - Fire,
// 4 - Base