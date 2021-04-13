using UnityEngine;

namespace _Project.Scripts.Enemies
{
	public class EnemyMovement : MonoBehaviour
	{
		[Header("Enemy Configuration")]
		[SerializeField] private float     speed         = 1.0f;
		[SerializeField] private float     rotationSpeed = 2.5f;
		private                  Rigidbody _rb;
		private                  Transform _target;
		
		private void Start()
		{
			_target = GameObject.FindWithTag("Player").transform;
			_rb     = GetComponent<Rigidbody>();
		}

		private void FixedUpdate()
		{
			if (!_target)
				return;
			Vector3 direction = (_target.position - transform.position).normalized;
			_rb.velocity = direction * speed;
			Quaternion rotation = Quaternion.LookRotation(direction);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
		}
	}
}