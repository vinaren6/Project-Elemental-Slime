using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Player;
using UnityEngine;

namespace _Project.Scripts.Events
{
	public class EnableAndDisableBasedOnPlayerElement : MonoBehaviour
	{
		[SerializeField] private bool                       equal = true;
		[SerializeField] private ElementalSystemTypeCurrent type;
		[SerializeField] private GameObject                 target;

		private void Start()
		{
			PlayerElementalSystemSwitchElement.StaticPlayerElementType.onElementTypeChange.AddListener(Change);
			Change();
		}

		private void Change() => target.SetActive(
			equal
				? type.Type == PlayerElementalSystemSwitchElement.StaticPlayerElementType.Type
				: type.Type != PlayerElementalSystemSwitchElement.StaticPlayerElementType.Type);
	}
}