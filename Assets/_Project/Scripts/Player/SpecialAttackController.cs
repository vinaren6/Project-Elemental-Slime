using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Events;
using _Project.Scripts.HealthSystem;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class SpecialAttackController : MonoBehaviour
	{
		[SerializeField] private ElementalSystemTypeCurrent type;
		[SerializeField] private float                      specialAttackMultiplier = 5f;

		private void OnEnable()
		{
			GetComponent<DestroyThisObj>(). DestroyMe(4f);
		}

		private void OnCollisionEnter(Collision other)
		{
			if (other.collider.TryGetComponent(out IHealth health))
				health.ReceiveDamage(type.Type, PlayerController.PlayerDamage * specialAttackMultiplier);
			
			GetComponent<DestroyThisObj>(). DestroyMe();
		}
	}
}