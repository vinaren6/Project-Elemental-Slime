using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Enemies.AI;
using _Project.Scripts.HealthSystem;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class ProjectileController : MonoBehaviour
	{
		[SerializeField] private ElementalSystemTypeCurrent type;
		
		private void OnEnable()
		{
			GetComponent<ElementalSystemColorShift>().SetColor();
		}

		private void OnDisable()
		{
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<ElementalSystemColorShift>().ResetColor();
		}

		private void OnCollisionEnter(Collision other)
		{
			if (!other.gameObject.CompareTag("Enemy"))
				return;

			if (other.collider.TryGetComponent(out IHealth health))
				health.ReceiveDamage(type.Type, PlayerController.PlayerDamage);

			if (PlayerController.IsDealingDamageOverTime && other.collider.TryGetComponent(out EnemyController enemy))
				enemy.StartDamageOverTime(type.Type, PlayerController.DamageOverTimeTotalTicks);

			gameObject.SetActive(false);
		}
	}
}