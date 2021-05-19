using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.FX
{
	public class UIDamageScreenGlowEffect : MonoBehaviour
	{
		[SerializeField] private Image  image;
		[SerializeField] private Health health;

		private float _baseHp = -1, _tempDamage;

		private void Awake()
		{
			health.onReceiveDamage.AddListener(Damage);
			health.onReceiveHealth.AddListener(Heal);
		}

		private void FixedUpdate()
		{
			image.color = new Color(image.color.r, image.color.g, image.color.b, _baseHp + _tempDamage);
			if (_tempDamage <= 0)
				_tempDamage = 0;
			else
				_tempDamage -= Time.fixedDeltaTime;
		}

		private void OnDestroy()
		{
			health.onReceiveDamage.RemoveListener(Damage);
			health.onReceiveHealth.RemoveListener(Heal);
		}

		private void Damage(float normalizedHealth)
		{
			_baseHp = -(normalizedHealth * 2 -
			            1);      //convertion: '0 -> 1' to '-1 -> 1'. this will only show effect when hp < 50%
			_tempDamage = 0.25f; //will show effect when hp < 75%
		}

		private void Heal(float normalizedHealth) =>
			_baseHp = -(normalizedHealth * 2 - 1);
	}
}