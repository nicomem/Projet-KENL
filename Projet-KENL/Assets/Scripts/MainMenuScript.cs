using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

    public void Load_Choose()
    {
        Application.LoadLevel("MainMenuChoose");
    }
	
	public void Load_MainMenu()
    {
        Application.LoadLevel("MainMenu");
    }
    public void Load_Settings()
    {
        Application.LoadLevel("MainMenuSettings");
    }
    public void Load_Credits()
    {
        Application.LoadLevel("MainMenuCredits");
    }
    public void Load_Training()
    {
        Application.LoadLevel("Training");
    }
    public void Load_NormalGame()
    {
        Application.LoadLevel("Plateforme");
    }
    
}
