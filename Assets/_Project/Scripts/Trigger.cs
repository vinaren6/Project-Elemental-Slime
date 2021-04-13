using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts
{
	public class Trigger : MonoBehaviour
	{
		public  UnityEvent onTriggerEnter;
		public  UnityEvent onTriggerExit;
		private void       OnTriggerEnter(Collider other) => onTriggerEnter.Invoke();
		private void       OnTriggerExit(Collider  other) => onTriggerEnter.Invoke();
	}
}