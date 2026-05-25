using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour {
    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void OnStartClick() {
        SceneManager.LoadScene("Level Selector");
    } 
    public void OnExitClick() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif 
        Application.Quit(); 
    }
    public void OnLevel1() {
        SceneManager.LoadScene("Level 1");
    }
    public void OnLevel2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void OnLevel3()
    {
        SceneManager.LoadScene("Level 3");
    }
}