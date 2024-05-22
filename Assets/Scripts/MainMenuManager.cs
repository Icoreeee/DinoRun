using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartMultiplayer()
    {
        SceneManager.LoadScene(1);
    }
    
    public void StartSinglePlayer()
    {
        SceneManager.LoadScene(2);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
