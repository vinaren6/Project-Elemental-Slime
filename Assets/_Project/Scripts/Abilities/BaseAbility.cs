using UnityEngine;

namespace _Project.Scripts.Abilities
{
	public class BaseAbility : MonoBehaviour, IAbility
	{
		public void Initialize(string newTag, float damage, Collider selfCollider = null)
		{
			// print("BaseAbility Initialized!");
		}

		public void Execute()
		{
			// print("BaseAbility Executed!");
		}
		
		public void Stop()
		{
			//print("BaseAbility Stopped!");
		}
	}
}