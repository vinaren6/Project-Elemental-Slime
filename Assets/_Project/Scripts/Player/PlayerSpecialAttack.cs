using _Project.Scripts.ElementalSystem;
using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class PlayerSpecialAttack : MonoBehaviour
	{
		[SerializeField] private AudioClip specialSFX;
		private float _nextSpecialAttack = 0;
		
		public void Activate(GameObject specialAttack, float projectileSpeed, float specialAttackCooldownTime, ElementalSystemTypeCurrent type)
		{
			if (Time.time < _nextSpecialAttack)
				return;
			
			GameObject projectile = Instantiate(specialAttack, transform.position, Quaternion.identity);
			projectile.GetComponent<ElementalSystemTypeCurrent>().Type = type.Type;
			projectile.GetComponent<Rigidbody>().AddForce(transform.forward * (projectileSpeed * 1.2f));
			_nextSpecialAttack = Time.time + specialAttackCooldownTime;
			ServiceLocator.HUD.SpecialAttack?.StartCooldown(specialAttackCooldownTime);
			ServiceLocator.Audio.PlaySFX(specialSFX);
		}
	}
}