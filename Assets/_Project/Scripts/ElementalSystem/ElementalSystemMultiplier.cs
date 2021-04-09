namespace _Project.Scripts.ElementalSystem
{
	public static class ElementalSystemMultiplier
	{
		public static float GetMultiplier(ElementalSystemTypes baseType, ElementalSystemTypes targetType) =>
			baseType == ElementalSystemTypes.Base || targetType == ElementalSystemTypes.Base
				? 1
				: 1 + (int) targetType % 4 - (int) baseType % 4 % 2;
	}
}