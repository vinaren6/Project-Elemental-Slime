using _Project.Scripts.HealthSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.WaveSystem
{
	public class SafeOnDeath : MonoBehaviour
	{
		public  Health      hp;
		private UnityAction _func;

		public void Set(UnityAction f)
		{
			_func = f;
			hp.onDeath.AddListener(Run);
		} // ReSharper disable Unity.PerformanceAnalysis
		private void Run()
		{
			_func.Invoke();
			hp.onDeath.RemoveListener(Run);
		}
	}
}