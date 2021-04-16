using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.ElementalSystem;
using UnityEngine;

namespace _Project.Scripts.Player
{
	public class ProjectilePool : MonoBehaviour
	{
		[SerializeField] private       GameObject projectilePrefab;
		[SerializeField] private       int        poolSize            = 10;
		[SerializeField] private       float      _projectileLifespan = 4f;

		[SerializeField] private ElementalSystemTypeCurrent type;

		private Queue<GameObject> _projectilePool;

		private void Start()
		{
			_projectilePool = new Queue<GameObject>();

			for (int i = 0; i < poolSize; i++) {
				GameObject projectile = Instantiate(projectilePrefab, transform);
				projectile.SetActive(false);
				_projectilePool.Enqueue(projectile);
			}
		}

		public GameObject SpawnFromPool()
		{
			GameObject projectile = _projectilePool.Dequeue();
			projectile.SetActive(true);
			projectile.GetComponent<ElementalSystemTypeCurrent>().Type = type.Type;
			projectile.transform.SetParent(null);
			StartCoroutine(ReturnProjectileToPool(projectile));
			return projectile;
		}

		private IEnumerator ReturnProjectileToPool(GameObject projectile)
		{
			yield return new WaitForSeconds(_projectileLifespan);
			projectile.SetActive(false);
			projectile.transform.SetParent(transform);
			projectile.transform.localPosition    = Vector3.zero;
			projectile.transform.localEulerAngles = Vector3.zero;
			_projectilePool.Enqueue(projectile);
		}
	}
}