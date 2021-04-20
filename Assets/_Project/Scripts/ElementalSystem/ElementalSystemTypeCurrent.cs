using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.ElementalSystem
{
	public class ElementalSystemTypeCurrent : MonoBehaviour
	{
		[SerializeField] private ElementalSystemTypes type;
		public                   UnityEvent           onElementTypeChange;

		public ElementalSystemTypes Type {
			get => type;
			set {
				type = value;
				onElementTypeChange.Invoke();
			}
		}

		public virtual void Destroy() => Destroy(gameObject);
	}
}