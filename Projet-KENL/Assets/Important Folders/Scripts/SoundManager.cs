using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioSource introtheme;
    public AudioSource fighttheme;
    public AudioSource volcanchoises;
    public AudioSource NightForest;

    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Sound Manager") != gameObject)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnLevelWasLoaded(int level)
    {
        string activeScene = SceneManager.GetActiveScene().ToString();

        switch (activeScene) {
            case "MainMenu":
                fighttheme.Stop();
                NightForest.Stop();
                volcanchoises.Stop();
                introtheme.Play();
                break;

            case "SingleplayerLobby":
                fighttheme.Stop();
                introtheme.Stop();
                NightForest.Stop();
                volcanchoises.Play();

                break;

            case "MultiplayerLobby":
                fighttheme.Stop();
                introtheme.Stop();
                NightForest.Stop();
                volcanchoises.Play();

                break;

            case "Bundok":
                introtheme.Stop();
                NightForest.Stop();
                volcanchoises.Stop();
                fighttheme.Play();
                break;

            case "Gutabaraw":
                introtheme.Stop();
                NightForest.Stop();
                volcanchoises.Stop();
                fighttheme.Play();
                break;

            case "Gubatadgi":
                introtheme.Stop();
                fighttheme.Stop();
                volcanchoises.Stop();
                NightForest.Play();
                break;

            case "Lungsod":
                introtheme.Stop();
                NightForest.Stop();
                volcanchoises.Stop();
                fighttheme.Play();
                break;
        }
    }
}
