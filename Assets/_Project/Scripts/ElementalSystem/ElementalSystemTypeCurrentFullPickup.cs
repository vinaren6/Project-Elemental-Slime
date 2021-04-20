using System.Collections;
using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public class ElementalSystemTypeCurrentFullPickup : ElementalSystemTypeCurrent
	{
		[SerializeField] private bool respawn = true;

		public override void Destroy()
		{
			if (respawn)
				Destroy(gameObject);
			else
				HideForT(gameObject, 10);
		}

		private IEnumerator HideForT(GameObject obj, float t)
		{
			obj.SetActive(false);
			yield return new WaitForSeconds(t);
			obj.SetActive(true);
		}
	}
}