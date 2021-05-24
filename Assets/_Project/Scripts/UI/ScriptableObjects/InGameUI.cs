using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.ScriptableObjects
{
	[CreateAssetMenu(fileName = "NewInGameUI", menuName = "InGameUI", order = 0)]
	public class InGameUI : ScriptableObject
	{
		[System.Serializable]    
		public class DamageNumber 
		{
			public string   name;
			public float    fontSizeModifier;
			public Material fontMaterial;
			public Color    color;
		}

		public List<DamageNumber> damageNumbers = new List<DamageNumber>();
		public TMP_FontAsset      inGameFont;
		public Color              enemyDamageColor;
		public Color              playerDamageColor;
		
		public float damageNumberFontSizeBase = 1f;
		public float killTextFontSize         = 1.3f;
		public Color killTextColor;
		public float comboTextFontSize = 1.5f;
		public Color comboTextColor;
		
		public float GetDamageNumberFontSize(string effectiveType) 
		{
			DamageNumber entry = damageNumbers.Find(c => c.name == effectiveType);
			if (entry != null) {
				float fontSize = damageNumberFontSizeBase * entry.fontSizeModifier;
				return fontSize;
			}
			Debug.Log("Couldn't get damageNumber.fontSizeModifier from InGameUI");
			return damageNumberFontSizeBase;
		}

		public Color GetDamageNumberColor(DamageType damageType)
		{
			return damageType switch {
				DamageType.Enemy  => enemyDamageColor,
				DamageType.Player => playerDamageColor,
				DamageType.Heal   => damageNumbers.Find(c => c.name == "Heal").color,
				_                 => Color.white
			};
		}
		
		public Material GetDamageNumberMaterial(string effectiveType) 
		{
			DamageNumber entry = damageNumbers.Find(c => c.name == effectiveType);
			if (entry != null) {
				return entry.fontMaterial;
			}
			Debug.Log("Couldn't get damageNumber.color from InGameUI");
			return null;
		}
	}
}