using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIScript : MonoBehaviour {
    private bool isPaused;
	// Use this for initialization
	void Start () {
        isPaused = false;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            isPaused = !isPaused;
        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1.0f;
    }
    private void OnGUI()
    {
        if (isPaused) 
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 320, Screen.height / 2 - 120, 640, 80), "Continue"))
                isPaused = false;
            if (GUI.Button(new Rect(Screen.width / 2 - 320, Screen.height / 2 + 80, 640, 80), "Back to main menu" ))
                {
                //Application.Quit();
                SceneManager.LoadScene("MainMenu");
            }
        }
 
    }
}
