using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Events
{
	public class DestroyEvent : MonoBehaviour
	{
		public UnityEvent onDestroy;

		private void OnDestroy() => onDestroy.Invoke();
	}
}