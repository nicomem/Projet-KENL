using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    private Dictionary<string, AudioSource> musics;
    private Dictionary<string, AudioSource> bruitagesAS;
    private AudioSource activeMusic;

    [Header("Musics")]
    public AudioSource introTheme;
    public AudioSource fightTheme;
    public AudioSource volcanChoices;
    public AudioSource nightForest;

    [Header("Bruitages")]
    public AudioSource attackSound;
    public AudioSource respawnSound;

    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Sound Manager") != gameObject)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        musics = new Dictionary<string, AudioSource> {
            { "MainMenu", introTheme },
            { "MainMenuChoose", introTheme },
            { "MainMenuCredits", introTheme },
            { "MainMenuMultiplayer", introTheme },
            { "SingleplayerLobby", volcanChoices },
            { "MultiplayerLobby", volcanChoices },
            { "Bundok", fightTheme },
            { "Gubataraw", fightTheme },
            { "Gubatgabi", nightForest },
            { "Lungsod", fightTheme }
        };

        bruitagesAS = new Dictionary<string, AudioSource> {
            { "Attack", attackSound },
            { "Respawn", respawnSound }
        };

        activeMusic = musics["MainMenu"];
        activeMusic.Play();
    }

    private void OnLevelWasLoaded(int level)
    {
        string activeScene = SceneManager.GetActiveScene().name;
        AudioSource newMusic;

        if (activeScene == "MainMenu")
            return;

        if (musics.TryGetValue(activeScene, out newMusic)
            && newMusic != activeMusic) {
            if (activeMusic.isPlaying)
                activeMusic.Stop();
            newMusic.Play();
            activeMusic = newMusic;
        }
    }

    public void DoBruitages(string soundCode)
    {
        AudioSource bruitage;

        if (bruitagesAS.TryGetValue(soundCode, out bruitage)
            && !bruitage.isPlaying)
            bruitagesAS[soundCode].Play();
    }
}
