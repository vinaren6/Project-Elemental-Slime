using System.Globalization;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI.Score
{
	[RequireComponent(typeof(TMP_Text))]
	public class StatTextDisplayInt : MonoBehaviour
	{
		[SerializeField] private string statKey;

		private void Start() => GetComponent<TMP_Text>().text = ToString(PlayerPrefs.GetInt(statKey));

		public static string ToString(int num)
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