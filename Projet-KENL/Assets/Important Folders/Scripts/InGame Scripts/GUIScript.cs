using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIScript : MonoBehaviour {

    private bool isPaused;
    private int buttonX, button1Y, button2Y, buttonWidth, buttonHeight;

    private void Start()
    {
        isPaused = false;

        buttonWidth = Screen.width / 2;
        buttonHeight = Screen.height / 10;

        buttonX = (Screen.width - buttonWidth) / 2;
        button1Y = (int)(0.4f * Screen.height);
        button2Y = (int)(0.6f * Screen.height);
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;

            if (isPaused)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1.0f;
        }
    }

    private void OnGUI()
    {
        if (isPaused) {
            if (GUI.Button(new Rect(buttonX, button1Y, buttonWidth, buttonHeight),
                           "Continue"))
                isPaused = false;

            if (GUI.Button(new Rect(buttonX, button2Y, buttonWidth, buttonHeight),
                            "Back to main menu"))
                GoToMainMenu();
        }
    }

    private void GoToMainMenu()
    {
        var mapInfosScript = GameObject.Find("Map Infos")
            .GetComponent<MapInfosScript>();

        // We destroy them by hand or else they'll reappear in multi mode
        // \- don't ask me why...
        foreach (GameObject go in mapInfosScript.ListPlayers)
            Destroy(go);

        SceneManager.LoadScene("MainMenu");
    }
}
