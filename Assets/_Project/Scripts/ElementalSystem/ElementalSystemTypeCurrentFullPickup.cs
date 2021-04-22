using System.Collections;
using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public class ElementalSystemTypeCurrentFullPickup : ElementalSystemTypeCurrent
	{
		[SerializeField] private bool  respawn      = true;
		[SerializeField] private float respawnDelay = 10;

		public override void Destroy()
		{
			if (respawn)
				Destroy(gameObject);
			else
				HideForT(gameObject, respawnDelay);
		}

		private IEnumerator HideForT(GameObject obj, float t)
		{
			obj.SetActive(false);
			yield return new WaitForSeconds(t);
			obj.SetActive(true);
		}
	}
}