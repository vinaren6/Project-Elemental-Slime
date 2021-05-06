using _Project.Scripts.Managers;
using UnityEngine;

namespace _Project.Scripts.UI
{
	public class GameOverUI : MonoBehaviour
	{
		private void OnEnable()
		{
			ServiceLocator.Game.SetPause(true);
		}
	}
}