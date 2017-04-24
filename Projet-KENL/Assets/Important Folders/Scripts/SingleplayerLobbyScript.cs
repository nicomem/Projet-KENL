using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SingleplayerLobbyScript : MonoBehaviour
{
    // Script derived from MenuMultiplayer, if you do a modification on
    // this last, make sure to update this script if necessary
    
    private GameObject canvas;
    private GameObject charaSelectBox;
    private GameObject readyButton;
    private GameObject mapSelectBox;
    private GameObject chooseMapButton;

    public GameObject[] persoPrefabs;
    public string[] persosPrefabsNames;
    public string[] mapScenes;
    public Texture[] mapScreenshots;

    private string persoName;
    private PlayerType playerSelected = 0;
    private int mapSelected = 0;

    private bool[] enabledIA = new bool[3];
    private PlayerType[] playerSelectedIA = new PlayerType[3];

    private GameObject charaSelected;

    public enum PlayerType { PlayerTest, StealthChar, Antiope };

    private void Start()
    {
        SetupLobbySceneButtons();
    }

    #region Perso Selection
    public void Lobby_BackButton()
    {
        // When clicking on "Back" button

        SceneManager.LoadScene("MainMenuChoose");
    }

    public void Lobby_ReadyButton()
    {
        UpdatePersoInSelect();
        UpdateMapInSelect();

        charaSelectBox.SetActive(false);

        // Map selection Box
        mapSelectBox.SetActive(true);

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
                    new Vector3(3f, 3f, 3f);
                charaSelected.transform.localPosition +=
                    new Vector3(0, -2.5f, 0);
                charaSelected.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;

            case PlayerType.PlayerTest:
                charaSelected.transform.localScale =
                    new Vector3(1.5f, 2f, 1f);
                break;

            case PlayerType.Antiope:
                charaSelected.transform.localScale =
                    new Vector3(3f, 3f, 3f);
                charaSelected.transform.position +=
                    new Vector3(0, -1.2f, 0);
                charaSelected.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;

            default:
                Debug.Log("[ERR] SingleplayerLobbyScript/UpdatePersoInSelect: " +
                    "Unrecognized character");
                break;
        }

        persoName = persosPrefabsNames[(int)playerSelected];

        playerScript.persoName = persoName;
        charaSelectBox.transform.Find("Panel - Perso Name").Find("Perso Name")
            .GetComponent<Text>().text = persoName;
    }
    #endregion

    #region Map Selection
    public void Lobby_MapSelectLeftArrow()
    {
        mapSelected = mapSelected - 1;
        if (mapSelected < 0)
            mapSelected = mapSelected + mapScenes.Length;

        UpdateMapInSelect();
    }

    public void Lobby_MapSelectRightArrow()
    {
        mapSelected = (mapSelected + 1) % mapScenes.Length;

        UpdateMapInSelect();
    }

    public void Lobby_MapSelectChooseButton()
    {
        // Start game (we give selected map)
        StartGame();
    }

    public void UpdateMapInSelect()
    {
        // Change map in image & sync with clients
        mapSelectBox.transform.Find("Map Name Panel").Find("Map Name")
            .GetComponent<Text>().text = mapScenes[mapSelected];
        mapSelectBox.transform.Find("Map Image").GetComponent<RawImage>().texture =
            mapScreenshots[mapSelected];
    }
    #endregion

    public void StartGame()
    {
        // Will be detroyed after (look a bit under)
        DontDestroyOnLoad(gameObject);

        Debug.Log("[INF] Starting game");
        SceneManager.LoadScene(mapScenes[mapSelected]);
    }

    private void OnLevelWasLoaded(int level)
    {
        GameObject go;

        // Player
        switch (persoName) {
            case "Gianluigi Conti":
                go = Instantiate(persoPrefabs[(int)PlayerType.StealthChar]);
                break;

            case "Player Test":
                go = Instantiate(persoPrefabs[(int)PlayerType.PlayerTest]);
                break;

            case "Antiope":
                go = Instantiate(persoPrefabs[(int)PlayerType.Antiope]);
                break;

            default:
                go = new GameObject();
                Debug.Log("[ERR] StartGame: Unrecognized persoName: " +
                    persoName);
                return;
        }

        go.GetComponent<PlayerScript>().persoName = persoName;

        // IA
        for (int i = 0; i < 3; i++) {
            if (enabledIA[i]) {
                go = Instantiate(persoPrefabs[(int)playerSelectedIA[i]]);
                var playerScript = go.GetComponent<PlayerScript>();
                playerScript.persoName =
                    persosPrefabsNames[(int)playerSelectedIA[i]];
                playerScript.isIA = true;
            }
        }

        // See, I told you this will be destroyed
        Destroy(gameObject);
    }

    private void SetupLobbySceneButtons()
    {
        // Get important GO (and set things)
        canvas = GameObject.Find("Canvas");
        charaSelectBox = canvas.transform.Find("Character Selection")
            .gameObject;

        mapSelectBox = canvas.transform.Find("Map Selection").gameObject;

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
    }

    #region IA functions
    public void ButtonEnableIA1()
    {
        ButtonEnableIA(0);
    }

    public void ButtonEnableIA2()
    {
        ButtonEnableIA(1);
    }

    public void ButtonEnableIA3()
    {
        ButtonEnableIA(2);
    }

    private void ButtonEnableIA(int IANumber)
    {
        enabledIA[IANumber] = !enabledIA[IANumber];

        var IASelection = canvas.transform.Find("IA Selection " + (IANumber + 1));
        var text = IASelection.Find("Button").Find("Text")
            .GetComponent<Text>().text;

        if (text == "Disabled")
            text = "Enabled";
        else
            text = "Disabled";

        IASelection.Find("Button").Find("Text")
            .GetComponent<Text>().text = text;
    }

    public void ButtonLeftIA1()
    {
        ButtonLeftIA(0);
    }

    public void ButtonRightIA1()
    {
        ButtonRightIA(0);
    }
    
    public void ButtonLeftIA2()
    {
        ButtonLeftIA(1);
    }

    public void ButtonRightIA2()
    {
        ButtonRightIA(1);
    }

    public void ButtonLeftIA3()
    {
        ButtonLeftIA(2);
    }

    public void ButtonRightIA3()
    {
        ButtonRightIA(2);
    }

    private void ButtonLeftIA(int IANumber)
    {
        playerSelectedIA[IANumber] = (PlayerType)((int)playerSelectedIA[IANumber] - 1);
        if (playerSelectedIA[IANumber] < 0)
            playerSelectedIA[IANumber] = (PlayerType)
                ((int)playerSelectedIA[IANumber] + persoPrefabs.Length);

        // Update perso in charSelectBox
        UpdateIA(IANumber);
    }

    private void ButtonRightIA(int IANumber)
    {
        playerSelectedIA[IANumber] = (PlayerType)
            (((int)playerSelectedIA[IANumber] + 1) % persoPrefabs.Length);

        // Update perso in charSelectBox
        UpdateIA(IANumber);
    }

    private void UpdateIA(int IANumber)
    {
        canvas.transform.Find("IA Selection " + (IANumber + 1))
            .Find("Panel - Perso Name").Find("Perso Name")
            .GetComponent<Text>().text = 
            persosPrefabsNames[(int)playerSelectedIA[IANumber]];
    }
    #endregion
}