using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuMultiplayer : NetworkManager
{
    private const int NUMBER_OF_PERSOS = 2;
    private const int NUMBER_OF_MAPS = 1;

    private Text InputIPAdress;
    private Text InputPlayerName;

    private GameObject canvas;
    private GameObject charaSelectBox;
    private GameObject readyButton;
    private GameObject mapSelectBox;
    private GameObject chooseMapButton;

    public string PlayerName { get; private set; }
    private bool isHost;
    private bool mapChoosed;
    private PlayerType playerSelected = 0;
    private MapType mapSelected = 0;

    [HideInInspector]
    public GameObject LobbyPlayer;
    [HideInInspector]
    public LobbyPlayerScript lobbyPlayerScript;
    private GameObject charaSelected;

    public enum PlayerType { PlayerTest, StealthChar };
    public enum MapType { Plateforme };
    public Vector3[] lobbySpawnPoints;

    // Server vars
    private NetworkConnection[] listPlayers = new NetworkConnection[4];

    #region MainMenuMultiplayer Scene

    public void StartHostButton()
    {
        // When clicking on "Start Host" button

        networkAddress = "127.0.0.1";
        PlayerName = InputPlayerName.text;
        isHost = true;

        if (PlayerName.Length == 0 || networkAddress.Length == 0)
            return;

        StartHost();
    }

    public void StartClientButton()
    {
        // When clicking on "Join Game" button

        networkAddress = InputIPAdress.text;
        PlayerName = InputPlayerName.text;
        isHost = false;

        if (PlayerName.Length == 0 || networkAddress.Length == 0)
            return;

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
        UpdatePersoInSelect();
        UpdateMapInSelect();

        // Set ready text in lobbyPlayer
        LobbyPlayer.transform.Find("IsReady Text")
            .GetComponent<Text>().text = "Ready!";

        LobbyPlayer.transform.Find("Perso Name")
            .gameObject.SetActive(true);

        charaSelectBox.SetActive(false);

        // Map selection Box
        mapSelectBox.SetActive(true);
        if (!isHost) {
            mapSelectBox.transform.Find("Left").gameObject.SetActive(false);
            mapSelectBox.transform.Find("Right").gameObject.SetActive(false);
            mapSelectBox.transform.Find("Choose").gameObject.SetActive(false);
            mapSelectBox.transform.Find("Map Image").gameObject.SetActive(false);
            mapSelectBox.transform.Find("Map Name Panel").gameObject.SetActive(false);
            mapSelectBox.transform.Find("Map Text").gameObject.SetActive(true);
        }

        // Change ready button
        Button button = readyButton.GetComponent<Button>();

        var buttonColors = button.colors;
        buttonColors.normalColor = new Color(255, 255, 0); // yellow
        buttonColors.pressedColor = new Color(200, 200, 0); // dark yelow
        buttonColors.highlightedColor = new Color(255, 255, 0);
        button.colors = buttonColors;

        Button.ButtonClickedEvent buttonEvent;
        buttonEvent = readyButton.GetComponent<Button>().onClick;
        buttonEvent.RemoveAllListeners();
        buttonEvent.AddListener(Lobby_StopReadyButton);
    }

    public void Lobby_StopReadyButton()
    {
        // When clicking on ready button when already ready (stop being ready)

        // Set not ready text in lobbyPlayer
        LobbyPlayer.transform.Find("IsReady Text")
            .GetComponent<Text>().text = "Not Ready";

        LobbyPlayer.transform.Find("Perso Name")
            .gameObject.SetActive(false);

        charaSelectBox.SetActive(true);

        // Map selection Box
        mapSelectBox.SetActive(false);

        // Change ready button
        Button button = readyButton.GetComponent<Button>();

        var buttonColors = button.colors;
        buttonColors.normalColor = new Color(255, 255, 255); // white
        buttonColors.pressedColor = new Color(200, 200, 200); // dark white
        buttonColors.highlightedColor = new Color(255, 255, 255);
        button.colors = buttonColors;

        Button.ButtonClickedEvent buttonEvent;
        buttonEvent = readyButton.GetComponent<Button>().onClick;
        buttonEvent.RemoveAllListeners();
        buttonEvent.AddListener(Lobby_ReadyButton);
    }

    public void Lobby_PlayerSelectLeftArrow()
    {
        playerSelected = (PlayerType)((int)playerSelected - 1);
        if (playerSelected < 0)
            playerSelected = (PlayerType)
                ((int)playerSelected + NUMBER_OF_PERSOS);

        // Update perso in charSelectBox
        UpdatePersoInSelect();
    }

    public void Lobby_PlayerSelectRightArrow()
    {
        playerSelected = (PlayerType)
            (((int)playerSelected + 1) % NUMBER_OF_PERSOS);

        // Update perso in charSelectBox
        UpdatePersoInSelect();
    }

    public void Lobby_MapSelectLeftArrow()
    {
        mapSelected = (MapType)((int)mapSelected - 1);
        if (mapSelected < 0)
            mapSelected = (MapType)
                ((int)mapSelected + NUMBER_OF_MAPS);

        UpdateMapInSelect();
    }

    public void Lobby_MapSelectRightArrow()
    {
        mapSelected = (MapType) (((int)mapSelected + 1) % NUMBER_OF_MAPS);

        UpdateMapInSelect();
    }

    public void Lobby_MapSelectChooseButton()
    {
        mapSelectBox.transform.Find("Left").gameObject.SetActive(false);
        mapSelectBox.transform.Find("Right").gameObject.SetActive(false);

        mapChoosed = true;

        // Change ready button
        Button button = chooseMapButton.GetComponent<Button>();

        var buttonColors = button.colors;
        buttonColors.normalColor = new Color(0, 200, 0);
        buttonColors.pressedColor = new Color(0, 150, 0);
        buttonColors.highlightedColor = new Color(0, 150, 0);
        button.colors = buttonColors;

        Button.ButtonClickedEvent buttonEvent;
        buttonEvent = chooseMapButton.GetComponent<Button>().onClick;
        buttonEvent.RemoveAllListeners();
        buttonEvent.AddListener(Lobby_MapSelectStopChooseButton);

        // Check if start game
        // ???
    }

    public void Lobby_MapSelectStopChooseButton()
    {
        mapSelectBox.transform.Find("Left").gameObject.SetActive(true);
        mapSelectBox.transform.Find("Right").gameObject.SetActive(true);

        mapChoosed = false;

        // Change ready button
        Button button = chooseMapButton.GetComponent<Button>();

        var buttonColors = button.colors;
        buttonColors.normalColor = new Color(255, 255, 255);
        buttonColors.pressedColor = new Color(200, 200, 200);
        buttonColors.highlightedColor = new Color(255, 255, 255);
        button.colors = buttonColors;

        Button.ButtonClickedEvent buttonEvent;
        buttonEvent = chooseMapButton.GetComponent<Button>().onClick;
        buttonEvent.RemoveAllListeners();
        buttonEvent.AddListener(Lobby_MapSelectChooseButton);
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        // When a client connects on the server
        // Function launched on the server

        base.OnServerConnect(conn);

        short indexPlayer = 0;

        for (short i = 0; i < 4; i++) {
            if (listPlayers[i] == null) {
                listPlayers[i] = conn;

                indexPlayer = i;
                Debug.Log("Player " + i + " connected");
                break;
            }
        }
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        short indexPlayer = -1;

        for (short i = 0; i < 4; i++) {
            if (listPlayers[i] == conn) {
                indexPlayer = i;
            }
        }

        if (indexPlayer == -1) {
            return;
        }

        // We create player prefab and give it to client
        //playerTypeList.Add(conn, 0);
        
        GameObject go = Instantiate(spawnPrefabs[0]);
        go.transform.Find("Panel Player").localScale = new Vector3(1, 1, 1);
        go.transform.Find("Panel Player").localPosition =
            lobbySpawnPoints[indexPlayer];

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

    public void UpdatePersoInSelect()
    {
        if (charaSelected != null) {
            Destroy(charaSelected);
        }

        charaSelected = Instantiate(spawnPrefabs[(int)playerSelected + 1]);
        charaSelected.transform.parent = charaSelectBox.transform;
        charaSelected.transform.position = canvas.transform.position;
        charaSelected.transform.localScale = new Vector3(1, 1, 1);

        string persoName = "";

        // Other fixes for each perso
        switch (playerSelected) {
            case PlayerType.StealthChar:
                charaSelected.transform.localScale = 
                    new Vector3(2.1f, 2.1f, 2.1f);
                charaSelected.transform.localPosition += 
                    new Vector3(0, -2f, 0);
                charaSelected.transform.Rotate(0, 180, 0);
                persoName = "Stealth Char";
                break;

            case PlayerType.PlayerTest:
                charaSelected.transform.localScale =
                    new Vector3(1.5f, 2f, 1f);
                persoName = "Player Test";
                break;

            default:
                break;
        }

        charaSelectBox.transform.Find("Panel - Perso Name").Find("Perso Name")
            .GetComponent<Text>().text = persoName;

        try {
            LobbyPlayer.transform.Find("Perso Name")
                .GetComponent<Text>().text = persoName;
        } catch (UnassignedReferenceException) {
        } catch (MissingReferenceException) {
        }


    }

    public void UpdateMapInSelect()
    {
        // Change map in image & sync with clients
        mapSelectBox.transform.Find("Map Name Panel").Find("Map Name")
            .GetComponent<Text>().text = mapSelected.ToString();
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
        // Get important GO (and set things)
        canvas = GameObject.Find("Canvas");
        charaSelectBox = canvas.transform.Find("Character Selection")
            .gameObject;

        mapSelectBox = canvas.transform.Find("Map Selection").gameObject;

        canvas.transform.Find("Character Selection").Find("Panel - Player Name")
            .Find("Player Name").GetComponent<Text>().text = PlayerName;

        // Set the buttons on "MuliplayerLobby" scene
        Button.ButtonClickedEvent button;

        button = GameObject.Find("Back").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_BackButton);

        // Perso selection

        readyButton = GameObject.Find("Ready");
        button = readyButton.GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_ReadyButton);

        button = charaSelectBox.transform.Find("Left").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_PlayerSelectLeftArrow);

        button = charaSelectBox.transform.Find("Right").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_PlayerSelectRightArrow);

        // Map selection

        button = mapSelectBox.transform.Find("Left").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_MapSelectLeftArrow);

        button = mapSelectBox.transform.Find("Right").GetComponent<Button>()
            .onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_MapSelectRightArrow);

        chooseMapButton = mapSelectBox.transform.Find("Choose").gameObject;
        button = chooseMapButton.GetComponent<Button>().onClick;
        button.RemoveAllListeners();
        button.AddListener(Lobby_MapSelectChooseButton);

        // Other things
        UpdatePersoInSelect();
        GameObject.Find("Main Camera").transform.position = 
            canvas.transform.position + new Vector3(0, 0, -10);
        mapChoosed = false;
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