using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Events
{
	public class Restart : MonoBehaviour
	{
		public void RestartNow() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}