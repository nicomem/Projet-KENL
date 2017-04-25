using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public Dictionary<string, GameObject> musicsGO;
    public GameObject activeMusicGO;

    public GameObject introTheme;
    public GameObject fightTheme;
    public GameObject volcanChoices;
    public GameObject nightForest;

    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Sound Manager") != gameObject)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        musicsGO = new Dictionary<string, GameObject> {
            { "MainMenu", introTheme },
            { "MainMenuMultiplayer", introTheme },
            { "SingleplayerLobby", volcanChoices },
            { "MultiplayerLobby", volcanChoices },
            { "Bundok", fightTheme },
            { "Gubataraw", fightTheme },
            { "Gubatgabi", nightForest },
            { "Lungsod", fightTheme }
        };

        activeMusicGO = musicsGO["MainMenu"];
        activeMusicGO.SetActive(true);
    }

    private void OnLevelWasLoaded(int level)
    {
        string activeScene = SceneManager.GetActiveScene().name;
        GameObject newMusicGO;

        if (musicsGO.TryGetValue(activeScene, out newMusicGO)
            && newMusicGO != activeMusicGO) {

            activeMusicGO.SetActive(false);
            newMusicGO.SetActive(true);
            activeMusicGO = newMusicGO;
        }
    }
}
