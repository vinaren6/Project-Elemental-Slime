using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.ElementalSystem
{
	public class PlayerElementalSystemTypeCurrent : MonoBehaviour
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
	}
}