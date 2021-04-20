using System.Collections;
using UnityEngine;

namespace _Project.Scripts.ElementalSystem
{
	public class ElementalSystemTypeCurrentFullPickup : ElementalSystemTypeCurrent
	{
		public override void Destroy() => HideForT(gameObject, 10);

		private IEnumerator HideForT(GameObject obj, float t)
		{
			obj.SetActive(false);
			yield return new WaitForSeconds(t);
			obj.SetActive(true);
		}
	}
}