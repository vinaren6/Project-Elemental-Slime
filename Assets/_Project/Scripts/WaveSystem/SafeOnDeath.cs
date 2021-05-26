using System;
using _Project.Scripts.HealthSystem;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.WaveSystem
{
	public class SafeOnDeath : MonoBehaviour
	{
		UnityAction   func;
		public Health hp;

		public void Set(UnityAction f)
		{
			func       =  f;
			hp.onDeath.AddListener(Run);
		}

		// ReSharper disable Unity.PerformanceAnalysis
		private void Run()
		{
			func.Invoke();
			hp.onDeath.RemoveListener(Run);
		}

	}
}