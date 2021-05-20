using UnityEngine;

namespace _Project.Scripts.Abilities
{
	public class BaseAbility : MonoBehaviour, IAbility
	{
		public bool StopLooping => false;
		
		public void Initialize(string newTag, float damage, Collider selfCollider = null)
		{
			// print("BaseAbility Initialized!");
		}

		public void Initialize(string newTag, float damage, float distance, Collider selfCollider = null)
		{
		}

		public bool DidExecute()
		{
			// print("BaseAbility Executed!");
			return true;
		}
		
		public void Stop(bool isPlayer = true)
		{
			//print("BaseAbility Stopped!");
		}

		public bool IsInRange()
		{
			return true;
		}

		public bool IsInWalkRange()
		{
			return true;
		}

		public bool CanAttack()
		{
			return true;
		}

		public float GetAttackTime()
		{
			return 0f;
		}
	}
}