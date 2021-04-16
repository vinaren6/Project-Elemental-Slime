using UnityEngine;

namespace _Project.Scripts.Events
{
	public class DestroyThisObj : MonoBehaviour
	{
		public void DestroyMe() => Destroy(gameObject);
		
		public void DestroyMe(float t) => Destroy(gameObject, t);
	}
}