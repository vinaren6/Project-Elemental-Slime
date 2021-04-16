using _Project.Scripts.ElementalSystem;
using _Project.Scripts.HealthSystem;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class ProjectileController : MonoBehaviour
	{
		[SerializeField] private ElementalSystemTypeCurrent type;
		
		private void OnCollisionEnter(Collision other)
		{
			if (other.collider.TryGetComponent(out IHealth health)) {
				health.ReceiveDamage(type.Type, 1);
			}
			gameObject.SetActive(false);
		}
	}
}