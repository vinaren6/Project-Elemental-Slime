using UnityEngine;

namespace _Project.Scripts.Abilities
{
	public class FireAbility : MonoBehaviour, IAbility
	{
		
		public void Initialize(float damage)
		{
			print("FireAbility Initialized!");
		}

		public void Execute()
		{
			print("FireAbility Executed!");
		}
	}
}