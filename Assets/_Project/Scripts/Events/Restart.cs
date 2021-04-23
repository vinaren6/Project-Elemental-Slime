using _Project.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Events
{
	public class Restart : MonoBehaviour
	{
		public void RestartNow()
		{
			ServiceLocator.Game.SetPause(false);
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}