using _Project.Scripts.ElementalSystem;
using UnityEngine;

namespace _Project.Scripts.Events
{
	public class SpawnObj : MonoBehaviour
	{

		[SerializeField] private GameObject                 prefab;
		[SerializeField] private ElementalSystemTypeCurrent type;
		
		public void Spawn()
		{
			GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
			if (obj.TryGetComponent(out ElementalSystemTypeCurrent prefabType))
				prefabType.Type = type.Type;
		}
	}
}