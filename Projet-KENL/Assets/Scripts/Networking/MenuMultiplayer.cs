using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuMultiplayer : NetworkManager
{
    private Text InputIPAdress;
    private Text InputPlayerName;

    private GameObject canvas;

    private string playerName;
    private bool isHost;

    private enum PlayerType { PlayerTest, StealthChar };
    private Dictionary<NetworkConnection, PlayerType> playerTypeList = 
        new Dictionary<NetworkConnection, PlayerType>();

    public Vector3[] lobbySpawnPoints;

    // Server vars
    private NetworkConnection[] listPlayers = new NetworkConnection[4];

    #region MainMenuMultiplayer Scene

    public void StartClientButton()
    {
        // When clicking on "Join Game" button

        networkAddress = InputIPAdress.text;
        playerName = InputPlayerName.text;
        isHost = false;

        StartClient();
        StartCoroutine(CheckClient(0.25f));
    }

    private IEnumerator CheckClient(float seconds)
    {
        // Wait for [seconds] & check if client is connected
        // If not, stop Unity from searching for a server

        yield return new WaitForSeconds(seconds);

        if (!client.isConnected)
            StopClient();
    }

    public void StartHostButton()
    {
        // When clicking on "Start Host" button

        networkAddress = InputIPAdress.text;
        playerName = InputPlayerName.text;
        isHost = true;


        StartHost();
    }

    public void Load_MainMenu()
    {
        // When clicking on "Main Menu" button

        SceneManager.LoadScene("MainMenu");
        Destroy(gameObject);
    }

    #endregion

    #region LobbyMultiplayer Scene

    public void Lobby_BackButton()
    {
        // When clicking on "Back" button

        if (isHost) {
            StopHost();
            listPlayers = new NetworkConnection[4];
        } else
            StopClient();
    }

    public void Lobby_ReadyButton()
    {
        // When clicking on "Ready" button
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        // When a client connects on the server
        // Function launched on the server

        base.OnServerConnect(conn);

        for (short i = 0; i < 4; i++) {
            if (listPlayers[i] == null) {
                listPlayers[i] = conn;

                Debug.Log("Player " + i + " connected");
                break;
            }
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        short indexPlayer = 0;

        for (short i = 0; i < 4; i++) {
            if (listPlayers[i] == conn) {
                indexPlayer = i;
            }
        }

         // We create player prefab and give it to client
        playerTypeList.Add(conn, 0);

        GameObject go = Instantiate(spawnPrefabs[0]);
        go.transform.parent = canvas.transform;
        go.transform.localPosition = lobbySpawnPoints[indexPlayer];
        go.transform.localScale = new Vector3(1, 1, 1);

        NetworkServer.SpawnWithClientAuthority(go, conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        // When a client disconnects from the server
        // Function launched on the server

        base.OnServerDisconnect(conn);

        for (short i = 1; i < 4; i++) {
            if (listPlayers[i] == conn) {
                listPlayers[i] = null;
                Debug.Log("Player " + i + " disconnected");
                break;
            }
        }
    }

    #endregion

    #region Other functions Scene

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When a scene is loaded
        // Launch functions for setting the buttons

        if (scene.name == "MultiplayerLobby") {
            // When entering the LobbyMultiplayer scene

            SetupLobbySceneButtons();
        } else if (scene.name == "MainMenuMultiplayer") {
            // When entering the MainMenuMultiplayer scene

            SetupMultiSceneButtons();
        } else {
            Debug.Log("Scene loaded (not wanted!): " + scene.name);
        }
    }

    private void SetupMultiSceneButtons()
    {
        // Set the buttons on "MainMenuMuliplayer" scene

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
        // Set the buttons on "MuliplayerLobby" scene

        Button.ButtonClickedEvent button;

        button = GameObject.Find("Back").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_BackButton);

        button = GameObject.Find("Ready").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_ReadyButton);

        canvas = GameObject.Find("Canvas");
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

    #endregion
}