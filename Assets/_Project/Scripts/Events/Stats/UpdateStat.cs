using UnityEngine;

namespace _Project.Scripts.Events.Stats
{
	public class UpdateStat : MonoBehaviour
	{

		[SerializeField] private string statKey;
		
		public void Add(int   value) => AddStat(statKey, value);
		public void Add(float value) => AddStat(statKey, value);
		
		public static void AddStat(string statKey , int value) => PlayerPrefs.SetInt(statKey, PlayerPrefs.GetInt(statKey) + value);
		public static void AddStat(string statKey , float value) => PlayerPrefs.SetFloat(statKey, PlayerPrefs.GetFloat(statKey) + value);

		public static void SetMaxStat(string statKey, int value)
		{
			if (PlayerPrefs.GetInt(statKey) < value) PlayerPrefs.SetInt(statKey, value);
		}
		public static void SetMaxStat(string statKey, float value)
		{
			if (PlayerPrefs.GetFloat(statKey) < value) PlayerPrefs.SetFloat(statKey, value);
		}

		public static void SetMaxAndAddStat(string statKeySet, string statKeyAdd, int value)
		{
			SetMaxStat(statKeySet, value);
			AddStat(statKeyAdd, value);
		}
		public static void SetMaxAndAddStat(string statKeySet, string statKeyAdd, float value)
		{
			SetMaxStat(statKeySet, value);
			AddStat(statKeyAdd, value);
		}
	}
}