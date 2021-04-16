using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject[] panels;
        public void PlayButtonClick()
        {
            SceneManager.LoadScene("TestSceneLinus");
        }

        public void OptionsButtonClick()
        {
        
        }

        public void CreditsButtonClick()
        {
        
        }

        public void QuitButtonClick()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
        }

        public void Reset()
        {
            Debug.Log("You have reset yo ass");
        }

    }
}
