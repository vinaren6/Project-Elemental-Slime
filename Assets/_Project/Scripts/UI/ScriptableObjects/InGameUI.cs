using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.UI.ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewInGameUI", menuName = "InGameUI", order = 0)]
	public class InGameUI : ScriptableObject
	{
		[System.Serializable]    
		public class DamageNumber 
		{
			public string name;
			public float  fontSize;
			public Color  color;
		}

		public List<DamageNumber> damageNumbers = new List<DamageNumber>();
		
		public float GetDamageNumberFontSize(string effectiveType) 
		{
			DamageNumber entry = damageNumbers.Find(c => c.name == effectiveType);
			if (entry != null) {
				return entry.fontSize;
			}
			Debug.Log("Couldn't get damageNumber.fontSize from InGameUI");
			return 1;
		}

		public Color GetDamageNumberColor(string effectiveType) 
		{ 
			// Debug.Log(effectiveType);
			DamageNumber entry = damageNumbers.Find(c => c.name == effectiveType);
			if (entry != null) {
				return entry.color;
			}
			return Color.white;
		}
	}
}