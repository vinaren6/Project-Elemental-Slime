using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Events
{
	public class LoadSceneEvent : MonoBehaviour
	{
		public void LoadScene(int         buildIndex) => SceneManager.LoadScene     (buildIndex);
		public void LoadScene(string      sceneName ) => SceneManager.LoadScene     (sceneName );
		public void LoadSceneAsync(int    buildIndex) => SceneManager.LoadSceneAsync(buildIndex);
		public void LoadSceneAsync(string sceneName ) => SceneManager.LoadSceneAsync(sceneName );
	}
}