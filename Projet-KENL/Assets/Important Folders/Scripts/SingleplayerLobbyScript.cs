using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SingleplayerLobbyScript : LobbyAbstract
{
    // Script derived from MenuMultiplayer, if you do a modification on
    // this last, make sure to update this script if necessary

    private string[] IAMode = new string[3];
    private PlayerType[] playerSelectedIA = new PlayerType[3];


    private void Start()
    {
        SetupLobbySceneButtons();

        for (int i = 0; i < IAMode.Length; i++)
            IAMode[i] = "Disabled";
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
    #endregion

    public void StartGame()
    {
        // Will be detroyed after (look a bit under)
        DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
        Debug.Log("[INF] Starting game");
#endif
        SceneManager.LoadScene(mapScenes[mapSelected]);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (string.IsNullOrEmpty(persoName))
            return;

        // Player
        int i = 0;
        for (i = 0; i < persosPrefabsNames.Length
            && persosPrefabsNames[i] != persoName; i++) ;

        if (i == persosPrefabsNames.Length)
            Debug.Log("[ERR] PersoName not found");

        GameObject go = Instantiate(persoPrefabs[i]);

        PlayerScript script = go.GetComponent<PlayerScript>();
        script.persoName = persoName;
        script.playerName = "Human";

        // IA
        for (i = 0; i < 3; i++) {
            if (IAMode[i] != "Disabled") {
                go = Instantiate(persoPrefabs[(int)playerSelectedIA[i]]);
                script = go.GetComponent<PlayerScript>();
                script.persoName = persosPrefabsNames[(int)playerSelectedIA[i]];
                script.playerName = string.Format("IA {0} ({1})", i, IAMode[i]);
                script.isIA = true;
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
        var IASelection = canvas.transform.Find("IA Selection " + (IANumber + 1));
        var text = IASelection.Find("Button").Find("Text")
            .GetComponent<Text>().text;

        switch (text) {
            case "Disabled": text = "Training"; break;
            case "Training": text = "Edwin"; break;
            case "Edwin": text = "Machine"; break;
            default: text = "Disabled"; break;
        }

        IAMode[IANumber] = text;

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