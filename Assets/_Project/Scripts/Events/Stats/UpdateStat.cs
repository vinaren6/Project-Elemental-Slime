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
	}
}