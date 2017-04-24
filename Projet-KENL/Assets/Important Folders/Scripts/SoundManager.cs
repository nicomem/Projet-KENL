using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnLevelWasLoaded(int level)
    {
        string activeScene = SceneManager.GetActiveScene().ToString();

        switch (activeScene) {
            case "MainMenu":
                break;

            case "SingleplayerLobby":
                break;

            case "MultiplayerLobby":
                break;

            case "Bundok":
                break;

            case "Gutabaraw":
                break;

            case "Gubatadgi":
                break;

            case "Lungsod":
                break;
        }
    }
}
