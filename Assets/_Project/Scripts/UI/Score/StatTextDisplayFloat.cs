using System.Globalization;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Score
{
	[RequireComponent(typeof(TMP_Text))]
	public class StatTextDisplayFloat : MonoBehaviour
	{
		[SerializeField] private string statKey;

		private void Start() => GetComponent<TMP_Text>().text = ToString(PlayerPrefs.GetFloat(statKey));
		
		public static string ToString(float num)
		{
			if (num > 999999999 || num < -999999999)
				return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
			if (num > 999999 || num < -999999)
				return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
			if (num > 999 || num < -999)
				return num.ToString("0,.#K", CultureInfo.InvariantCulture);
			return num.ToString(CultureInfo.InvariantCulture);
		}
	}
}