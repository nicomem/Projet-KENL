using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuMultiplayer : NetworkManager
{
    private const int NUMBER_OF_MAPS = 1;

    private Text InputIPAdress;
    private Text InputPlayerName;

    private GameObject canvas;
    private GameObject charaSelectBox;
    private GameObject readyButton;
    private GameObject mapSelectBox;
    private GameObject chooseMapButton;

    public GameObject[] lobbyPrefabs;
    public GameObject[] persoPrefabs;
    public string[] gameScenes;

    public string PlayerName { get; private set; }
    private string persoName;
    private bool isHost;
    private bool mapChoosed;
    private PlayerType playerSelected = 0;
    private MapType mapSelected = 0;

    [HideInInspector] public GameObject LobbyPlayer;
    [HideInInspector] public LobbyPlayerScript lobbyPlayerScript;
    private GameObject charaSelected;
    private int connectedPlayers;

    public enum PlayerType { PlayerTest, StealthChar };
    public enum MapType { Plateforme };

    // Server vars
    public NetworkConnection[] listNetworkConn = new NetworkConnection[4];
    public GameObject[] listPlayersGO = new GameObject[4];
    public bool[] isReadyPlayers = new bool[4];
    public string[] persoNames = new string[4];

    #region MainMenuMultiplayer Scene

    public void StartHostButton()
    {
        // When clicking on "Start Host" button

        networkAddress = "127.0.0.1";
        PlayerName = InputPlayerName.text;

        if (PlayerName.Length == 0 || networkAddress.Length == 0)
            return;

        isHost = true;
        connectedPlayers = 0;
        listNetworkConn = new NetworkConnection[4];
        listPlayersGO = new GameObject[4];
        isReadyPlayers = new bool[4];
        persoNames = new string[4];
        StartHost();
    }

    public void StartClientButton()
    {
        // When clicking on "Join Game" button

        networkAddress = InputIPAdress.text;
        PlayerName = InputPlayerName.text;

        if (PlayerName.Length == 0 || networkAddress.Length == 0)
            return;

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

    public void Load_MainMenu()
    {
        // When clicking on "Main Menu" button
        SceneManager.LoadScene("MainMenu");
    }

    #endregion

    #region LobbyMultiplayer Scene

    #region Perso Selection
    public void Lobby_BackButton()
    {
        // When clicking on "Back" button

        if (isHost) {
            StopHost();
            listNetworkConn = new NetworkConnection[4];
            listPlayersGO = new GameObject[4];
            isReadyPlayers = new bool[4];
            persoNames = new string[4];
        } else
            StopClient();
    }

    public void Lobby_ReadyButton()
    {
        UpdatePersoInSelect();
        UpdateMapInSelect();

        // Set ready text in lobbyPlayer
        lobbyPlayerScript.CmdSyncIsReady(true);
        lobbyPlayerScript.CmdSyncPersoName(persoName,
            lobbyPlayerScript.indexPlayer);

        charaSelectBox.SetActive(false);

        // Tell server that ready
        lobbyPlayerScript.CmdTellReady(true);

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

        Lobby_MapSelectStopChooseButton();

        // Set not ready text in lobbyPlayer
        lobbyPlayerScript.CmdSyncIsReady(false);
        lobbyPlayerScript.CmdSyncPersoName("", lobbyPlayerScript.indexPlayer);

        charaSelectBox.SetActive(true);

        // Map selection Box
        mapSelectBox.SetActive(false);

        // Tell to server that not ready
        lobbyPlayerScript.CmdTellReady(false);

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
                ((int)playerSelected + persoPrefabs.Length);

        // Update perso in charSelectBox
        UpdatePersoInSelect();
    }

    public void Lobby_PlayerSelectRightArrow()
    {
        playerSelected = (PlayerType)
            (((int)playerSelected + 1) % persoPrefabs.Length);

        // Update perso in charSelectBox
        UpdatePersoInSelect();
    }

    public void UpdatePersoInSelect()
    {
        if (charaSelected != null) {
            Destroy(charaSelected);
        }

        charaSelected = Instantiate(persoPrefabs[(int)playerSelected]);
        charaSelected.transform.parent = charaSelectBox.transform;
        charaSelected.transform.position = canvas.transform.position;
        charaSelected.transform.localScale = new Vector3(1, 1, 1);

        var playerScript = charaSelected.GetComponent<PlayerScript>();

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

        playerScript.persoName  = persoName;
        charaSelectBox.transform.Find("Panel - Perso Name").Find("Perso Name")
            .GetComponent<Text>().text = persoName;
    }
    #endregion

    #region Map Selection
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
        buttonColors.normalColor = new Color(255, 255, 0); // yellow
        buttonColors.pressedColor = new Color(200, 200, 0); // dark yelow
        buttonColors.highlightedColor = new Color(255, 255, 0);
        button.colors = buttonColors;

        Button.ButtonClickedEvent buttonEvent;
        buttonEvent = chooseMapButton.GetComponent<Button>().onClick;
        buttonEvent.RemoveAllListeners();
        buttonEvent.AddListener(Lobby_MapSelectStopChooseButton);

        // Check if start game
        UpdateReady();
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

    public void UpdateMapInSelect()
    {
        // Change map in image & sync with clients
        mapSelectBox.transform.Find("Map Name Panel").Find("Map Name")
            .GetComponent<Text>().text = mapSelected.ToString();
    }
    #endregion

    #region Start Game
    public void UpdateReady(int _indexPlayer = -1, bool isReady = false)
    {
        // Call it on server

        if (_indexPlayer >= 0 && _indexPlayer <= 3)
            isReadyPlayers[_indexPlayer] = isReady;

        if (mapChoosed){
            // I know, this is bad code, but you'll probably not see this
            for (short i = 0; i < connectedPlayers; i++) {
                if (!isReadyPlayers[i])
                    return;
            }

            string mapSceneString = null;

            switch (mapSelected) {
                case MapType.Plateforme:
                    mapSceneString = "Plateforme";
                    break;

                default:
                    Debug.Log("[ERR] UpdateReady: Map selected not recognized," +
                        " could not create string");
                    break;
            }

            if (mapSceneString != null)
                StartGame(mapSceneString);
        }
    }

    public void StartGame(string mapSceneString)
    {
        // Called on server
        // Will start the game on every client

        // Create all persos (DontDestroyOnLoad & authority)
        GameObject go;
        Debug.Log("[DEB] StartGame: ");

        for (short i = 0; i < 4; i++) {
            string persoName = persoNames[i];
            Debug.Log(persoName);

            if (persoName == "" || persoName == null)
                continue;

            Debug.Log("[DEB] StartGame: " + persoName);

            switch (persoName) {
                case "Stealth Char":
                    go = Instantiate(persoPrefabs[(int)PlayerType.StealthChar]);
                    break;

                case "Player Test":
                    go = Instantiate(persoPrefabs[(int)PlayerType.PlayerTest]);
                    break;

                default:
                    go = new GameObject();
                    Debug.Log("[ERR] StartGame: Unrecognized persoName: " +
                        persoName);
                    return;
            }

            NetworkServer.SpawnWithClientAuthority(go, listNetworkConn[i]);
            go.GetComponent<PlayerScript>().CmdSyncPersoName(persoName);
        }

        Debug.Log("[INF] Starting game");
        ServerChangeScene(mapSceneString);
    }
    #endregion

    #region Unity Global Interrupts
    public override void OnServerConnect(NetworkConnection conn)
    {
        // When a client connects on the server
        // Function launched on the server

        base.OnServerConnect(conn);

        for (short i = 0; i < 4; i++) {
            if (listNetworkConn[i] == null) {
                listNetworkConn[i] = conn;
                connectedPlayers++;

                Debug.Log("[INF] OnServerConnect: Player " + i +
                    " connected");
                return;
            }
        }

        // If already 4 players
        conn.Disconnect();
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

        short indexPlayer = 0;

        for (short i = 0; i < 4; i++) {
            if (listNetworkConn[i] == conn) {
                indexPlayer = i;
                break;
            }
        }

        listPlayersGO[indexPlayer] = Instantiate(lobbyPrefabs[indexPlayer]);
        NetworkServer.SpawnWithClientAuthority(listPlayersGO[indexPlayer], conn);
        listPlayersGO[indexPlayer].GetComponent<LobbyPlayerScript>()
            .CmdSyncIndexPlayer(indexPlayer);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        // When a client disconnects from the server
        // Function launched on the server

        base.OnServerDisconnect(conn);

        for (short i = 1; i < 4; i++) {
            if (listNetworkConn[i] == conn) {
                listNetworkConn[i] = null;
                connectedPlayers--;

                listPlayersGO[i].GetComponent<LobbyPlayerScript>()
                    .CmdDisconnect();

                Debug.Log("[INF] OnServerDisconnected: Player " + i +
                    " disconnected");
                break;
            }
        }
    }
    #endregion

    #endregion

    #region Multi-Scenes Functions

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // When a scene is loaded
        // Launch functions for setting the buttons

        Debug.Log("[INF] Loaded scene: " + scene.name);

        if (scene.name == "MainMenu") {
            // When entering the MainMenu scene
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
                if (go.transform.parent != null)
                    Destroy(go.transform.parent.gameObject);
                else
                    Destroy(go);
                
            }
            Lobby_BackButton();
            offlineScene = "MainMenu";
            SceneManager.LoadScene("MainMenu");
            Destroy(gameObject);
        } else if (scene.name == "MultiplayerLobby") {
            // When entering the LobbyMultiplayer scene
            SetupLobbySceneButtons();
        } else if (scene.name == "MainMenuMultiplayer") {
            // When entering the MainMenuMultiplayer scene
            SetupMultiSceneButtons();
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
        Camera.main.transform.position =
            new Vector3(canvas.transform.position.x,
                        canvas.transform.position.y,
                        -10);
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