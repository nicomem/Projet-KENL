using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMultiplayer : NetworkManager
{
    private Text InputIPAdress;
    private Text InputPlayerName;

    private string playerName;

    // ///////////////////////// //
    // MainMenuMultiplayer Scene //
    // ///////////////////////// //

    public void StartClientButton()
    {
        networkAddress = InputIPAdress.text;
        playerName = InputPlayerName.text;

        NetworkServer.Reset();
        StartClient();
    }

    public void StartHostButton()
    {
        networkAddress = InputIPAdress.text;
        playerName = InputPlayerName.text;

        NetworkServer.Reset();
        StartHost();
    }

    public void Load_MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    

    // ////////////////////// //
    // LobbyMultiplayer Scene //
    // ////////////////////// //

    public void Lobby_BackButton()
    {
        StopHost();
    }




    // ///////////////////// //
    // Other functions Scene //
    // ///////////////////// //

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MultiplayerLobby") {
            // When entering the LobbyMultiplayer scene
            Debug.Log("Scene loaded: " + scene.name);
            Debug.Log("Player entered lobby : " + playerName);

            SetupLobbySceneButtons();
        } else if (scene.name == "MainMenuMultiplayer") {
            // When entering the MainMenuMultiplayer scene
            Debug.Log("Scene loaded: " + scene.name);

            SetupMultiSceneButtons();
        } else {
            Debug.Log("Scene loaded (not wanted!): " + scene.name);
        }
    }

    private void SetupMultiSceneButtons()
    {
        Button.ButtonClickedEvent button;

        button = GameObject.Find("StartHost Button").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(StartHostButton);

        button = GameObject.Find("JoinGame Button").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(StartClientButton);

        button = GameObject.Find("MainMenu Button").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Load_MainMenu);

        InputIPAdress = GameObject.Find("IPAdress Text")
            .GetComponent<Text>();
        InputPlayerName = GameObject.Find("Player Name Text")
            .GetComponent<Text>();
    }

    private void SetupLobbySceneButtons()
    {
        Button.ButtonClickedEvent button;

        button = GameObject.Find("Back").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_BackButton);
    }

    void OnEnable()
    {
        // Tell our 'OnLevelFinishedLoading' function to start listening for
        // a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += SceneLoaded;
    }

    void OnDisable()
    {
        // Tell our 'OnLevelFinishedLoading' function to stop listening for
        // a scene change as soon as this script is disabled.
        SceneManager.sceneLoaded -= SceneLoaded;
    }
}