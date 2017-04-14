using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public void Load_Choose()
    {
        SceneManager.LoadScene("MainMenuChoose");
    }
	
	public void Load_MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Load_Settings()
    {
        SceneManager.LoadScene("MainMenuSettings");
    }

    public void Load_Credits()
    {
        SceneManager.LoadScene("MainMenuCredits");
    }

    public void Load_Training()
    {
        SceneManager.LoadScene("Training");
    }

    public void Load_NormalGame()
    {
        SceneManager.LoadScene("SingleplayerLobby");
    }

    public void Load_Multiplayer()
    {
        SceneManager.LoadScene("MainMenuMultiplayer");
    }

    public void Load_Exit()
    {
        Application.Quit();
    }
}
