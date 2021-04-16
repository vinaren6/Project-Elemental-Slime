using UnityEngine;

namespace _Project.Scripts.Player
{
	public class ProjectileController : MonoBehaviour
	{
		private void OnCollisionEnter(Collision other) => gameObject.SetActive(false);
	}
}