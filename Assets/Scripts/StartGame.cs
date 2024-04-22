using UnityEngine;

public class StartGame : MonoBehaviour
{

    private void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void StartTheGame()
    {
        LoadGame();
    }

    public void Exit()
    {
    #if UNITY_EDITOR
        // If running in the Unity Editor, stop playing
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}