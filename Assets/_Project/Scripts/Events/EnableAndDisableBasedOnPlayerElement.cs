using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Events
{
	public class EnableAndDisableBasedOnPlayerElement : MonoBehaviour
	{
		[SerializeField] private bool                 equal = true;
		[SerializeField] private ElementalSystemTypes type;
		[SerializeField] private GameObject           target;

		private void Start()
		{
			PlayerElementalSystemSwitchElement.StaticPlayerElementType.onElementTypeChange.AddListener(Change);
			Change();
		}

		private void Change() => target.SetActive(
			equal
				? type == PlayerElementalSystemSwitchElement.StaticPlayerElementType.Type
				: type != PlayerElementalSystemSwitchElement.StaticPlayerElementType.Type);
	}
}