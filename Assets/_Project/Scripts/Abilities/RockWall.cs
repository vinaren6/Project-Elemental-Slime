using UnityEngine;

namespace _Project.Scripts.Abilities
{
    public class RockWall : MonoBehaviour
    {
        private EarthAbility _ability;

        private float _damage;

        public void Initialize(EarthAbility ability, float damage)
        {
            _ability = ability;
            _damage = damage;
        }

        public void Execute(Transform spawnTransform)
        {
            
        }
        
        // private void OnTriggerEnter(Collider other)
        // {
        // 	if (other.TryGetComponent(out IHealth health)) {
        // 		float earthMultiplier = Mathf.Clamp(_attackTrigger.size.x, 5f, 10f) / 10;
        // 		health.ReceiveDamage(ElementalSystemTypes.Earth, earthMultiplier * _damage);
        // 	}
        // 	if (other.TryGetComponent(out NavMeshAgent agent)) {
        // 		StartCoroutine(HitPushBack(agent, _direction, _knockBackForce, 1f));
        // 	}
        // }
        
        // private IEnumerator HitPushBack(NavMeshAgent agent, Vector3 direction, float pushStrength, float rotationFreezeTime)
        // {
        //     if (agent == null)
        //         yield break;
			     //
        //     agent.velocity       = direction * pushStrength;
        //     agent.updateRotation = false;
        //     yield return new WaitForSeconds(rotationFreezeTime);
        //     agent.updateRotation = true;
        // }
    }
}
