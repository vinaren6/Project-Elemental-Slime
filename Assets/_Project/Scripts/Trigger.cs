using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts
{
	public class Trigger : MonoBehaviour
	{
		public  UnityEvent onTriggerEnter;
		private void       OnTriggerEnter(Collider other) { onTriggerEnter.Invoke(); }
	}
}