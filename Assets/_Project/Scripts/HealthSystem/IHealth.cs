using _Project.Scripts.ElementalSystem;

namespace _Project.Scripts.HealthSystem
{
	public interface IHealth
	{
		float HitPoints    { get; set; }
		float MaxHitPoints { get; set; }
		void  ReceiveDamage(ElementalSystemTypes damageType, float damage);
		void  ReceiveHealth(ElementalSystemTypes damageType, float damage);
	}
}