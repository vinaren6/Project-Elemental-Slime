using _Project.Scripts.ElementalSystem;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class PlayerSpecialAttack : MonoBehaviour
	{
		private float _nextSpecialAttack = 0;
		
		public void Activate(GameObject specialAttack, float projectileSpeed, float specialAttackRate, ElementalSystemTypeCurrent type)
		{
			if (!(Time.time > _nextSpecialAttack))
				return;
			GameObject projectile = Instantiate(specialAttack, transform.position, Quaternion.identity);
			projectile.GetComponent<ElementalSystemTypeCurrent>().Type = type.Type;
			projectile.GetComponent<Rigidbody>().AddForce(transform.forward * (projectileSpeed * 1.2f));
			_nextSpecialAttack = Time.time + specialAttackRate;
		}
	}
}