using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    private Dictionary<string, AudioSource> musics;
    private Dictionary<string, AudioSource> bruitagesAS;
    private AudioSource activeMusic;

    public Slider VolumeSlider;
    //private float volume;

    [Header("Musics")]
    public AudioSource introTheme;
    public AudioSource fightTheme;
    public AudioSource volcanChoices;
    public AudioSource nightForest;
    public AudioSource volcanTheme;
    public AudioSource beachTheme;

    [Header("Bruitages")]
    public AudioSource attackSound;
    public AudioSource respawnSound;

    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("Sound Manager") != gameObject)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        if (musics == null) {
            InitMusics();
            activeMusic = musics["MainMenu"];
            activeMusic.Play();
        }

        if (bruitagesAS == null)
            InitBruitages();
    }

    private void InitMusics()
    {
        musics = new Dictionary<string, AudioSource> {
            { "MainMenu", introTheme },
            { "MainMenuChoose", introTheme },
            { "MainMenuCredits", introTheme },
            { "MainMenuSettings", introTheme },
            { "MainMenuMultiplayer", introTheme },
            { "SingleplayerLobby", volcanChoices },
            { "MultiplayerLobby", volcanChoices },
            { "Bundok", fightTheme },
            { "Gubataraw", fightTheme },
            { "Gubatgabi", nightForest },
            { "Lungsod", fightTheme },
            { "Bulkan", volcanTheme },
            { "Tabingdagat", beachTheme }
        };
    }

    private void InitBruitages()
    {
        bruitagesAS = new Dictionary<string, AudioSource> {
            { "Attack", attackSound },
            { "Respawn", respawnSound }
        };
    }

    public void OnValueChanged()
    {
        AudioListener.volume = VolumeSlider.value;
    }

    private void OnLevelWasLoaded(int level)
    {
        string activeScene = SceneManager.GetActiveScene().name;
        AudioSource newMusic;

        if (activeScene == "MainMenuSettings") {
            VolumeSlider = GameObject.Find("Canvas").transform.Find("Slider Général")
                .GetComponent<Slider>();
            VolumeSlider.value = AudioListener.volume;
        }

        if (activeMusic == null)
            activeMusic = introTheme;

        if (musics == null)
            InitMusics();

        if (bruitagesAS == null)
            InitBruitages();

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
