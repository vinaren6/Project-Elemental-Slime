using UnityEngine;

namespace _Project.Scripts.WaveSystem
{
	public interface IWaveSpawner
	{
		bool IsActive { get; set; }
		bool Spawn(GameObject obj);
	}
}