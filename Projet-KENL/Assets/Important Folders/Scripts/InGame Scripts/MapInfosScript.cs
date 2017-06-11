using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapInfosScript : MonoBehaviour
{
    public GameObject[] ListPlayers { get; private set; }
    public PlayerScript[] ListPlayerScripts { get; private set; }
    public Vector3[] startPositions;
    public Vector3[] respawnPositions;

    public float xMinLimit, xMaxLimit, yMinLimit, yMaxLimit;

    private bool[] playersInitiated;
    private float currentY, currentX;
    public bool InitPlayersFinished { get; private set; }

    private bool gameHasEnded;

    private SoundManager soundManager;

    private void Start()
    {
        ListPlayers = GameObject.FindGameObjectsWithTag("Player");

        ListPlayerScripts = new PlayerScript[ListPlayers.Length];
        for (short i = 0; i < ListPlayers.Length; i++)
            ListPlayerScripts[i] = ListPlayers[i].GetComponent<PlayerScript>();

        playersInitiated = new bool[ListPlayers.Length];

        GameObject soundManagerGO = GameObject.Find("Sound Manager");
        if (soundManagerGO != null)
            soundManager = soundManagerGO.GetComponent<SoundManager>();

        InitMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (!InitPlayersFinished)
            InitPlayers();
        else {
            CheckEjected();
            if (!gameHasEnded) CheckVictory();
        }
    }

    private void InitMap()
    {
        Light light = GameObject.Find("Directional Light").GetComponent<Light>();

        light.transform.rotation = Quaternion.Euler(30, 0, 0);
    }

    private void InitPlayers()
    {
        for (ushort i = 0; i < ListPlayers.Length; i++) {
            var player = ListPlayers[i];
            var playerScript = ListPlayerScripts[i];
            var startPos = startPositions[i];

            if (!playersInitiated[i] && playerScript.persoName != null
                && playerScript.persoName != "") {
                // Remove DontDestroyOnLoad
                player.transform.SetParent(transform);
                player.transform.SetParent(null);

                GameObject parent;

                switch (playerScript.persoName) {
                    case "Gianluigi Conti":
                        // Set Rotate90 as parent (& do things)
                        parent = new GameObject("Gianluigi Conti - Rotate90");
                        parent.transform.position = Vector3.zero;
                        parent.transform.rotation = Quaternion.Euler(0, 90, 0);

                        player.transform.SetParent(parent.transform);
                        player.transform.localRotation = Quaternion.identity;
                        break;

                    case "Antiope":
                        parent = new GameObject("Antiope - Rotate90");
                        parent.transform.position = Vector3.zero;
                        parent.transform.rotation = Quaternion.Euler(0, 90, 0);

                        player.transform.SetParent(parent.transform);
                        player.transform.localRotation = Quaternion.identity;

                        player.transform.localScale = new Vector3(2f, 2f, 2f);
                        break;

                    case "Vladimir X":
                        parent = new GameObject("Vladimir X - Rotate90");
                        parent.transform.position = Vector3.zero;
                        parent.transform.rotation = Quaternion.Euler(0, 90, 0);

                        player.transform.SetParent(parent.transform);
                        player.transform.localRotation = Quaternion.identity;

                        player.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
                        break;

                    case "Satela":
                        parent = new GameObject("Satela - Rotate90");
                        parent.transform.position = Vector3.zero;
                        parent.transform.rotation = Quaternion.Euler(0, 90, 0);

                        player.transform.SetParent(parent.transform);
                        player.transform.localRotation = Quaternion.identity;

                        player.transform.localScale = new Vector3(2.25f, 2.25f, 2.25f);
                        break;

                    default:
                        Debug.Log("[ERR] MapInfosScript/InitPlayers: " +
                            "persoName not recognized : " + playerScript.persoName);
                        break;
                }

                player.transform.position = startPos;

                // Get IA mode + ')'
                string[] nameSplit = playerScript.playerName.Split('(');

                if (nameSplit.Length == 1) { // Human
                    player.AddComponent<CharaControlScript>();
                } else {
                    switch (nameSplit[1]) { // IAMode + ')'
                        case "Training)":
                            player.AddComponent<IATrainingScript>();
                            break;

                        case "Edwin)":
                            player.AddComponent<IAEdwinScript>();
                            break;

                        case "Machine)":
                            player.AddComponent<IAScript>();
                            break;

                        default:
                            Debug.Log("[ERR] IAMode not recognized");
                            break;
                    }
                }

                playersInitiated[i] = true;
            }
        }

        InitPlayersFinished = ListPlayers.Length > 0
            && playersInitiated.All(b => b);
    }

    private void CheckEjected()
    {
        for (short i = 0; i < ListPlayers.Length; i++) {
            var player = ListPlayers[i];
            var playerScript = player.GetComponent<PlayerScript>();
            currentY = player.transform.position.y;
            currentX = player.transform.position.x;

            bool ejected = currentY < yMinLimit || currentY > yMaxLimit
             || currentX < xMinLimit || currentX > xMaxLimit;

            bool ko = playerScript.percentHealth >= 100;

            if (ejected || ko) {
                if (!gameHasEnded) // Player will not die when end screen
                    --playerScript.persoLives; // Will be (maybe) synched auto

                if (playerScript.persoLives <= 0) {
                    playerScript.SyncIsKO(true);
                    // Do not destroy player but disable it
                    // So we can still get its infos
                    player.SetActive(false);
                } else {
                    /* Animation ejected */
                    playerScript.SetVerticalVelocity(0);
                    playerScript.SetHorizontalVelocity(0);
                    playerScript.percentHealth = 0;
                    player.GetComponent<PickUpObjects>().ResetBonuses();
                    player.transform.position = respawnPositions[i];

                    if (soundManager != null)
                        soundManager.DoBruitages("Respawn");
                }
            }
        }
    }

    private void CheckVictory()
    {
        // Check victory
        List<PlayerScript> activePlayerScripts = new List<PlayerScript>();
        foreach (var playerScript in ListPlayerScripts) {
            if (!playerScript.isKO)
                activePlayerScripts.Add(playerScript);
        }

        // If victory or tie
        gameHasEnded = activePlayerScripts.Count <= 1;
    }

    private void OnGUI()
    {
        Vector3 pos;
        foreach (PlayerScript script in ListPlayerScripts) {
            if (script == null || script.isKO)
                continue;

            pos = Camera.main.WorldToScreenPoint(script.transform.position);
            GUI.Label(new Rect(pos.x - (4 * script.playerName.Length),
                                Screen.height - pos.y - 115, 100, 25),
                script.playerName);
        }

        if (gameHasEnded) {
            string message = "The game has ended!\n\nBack to main menu";

            int buttonWidth = Screen.width / 3;
            int buttonHeight = Screen.height / 5;
            int buttonX = (Screen.width - buttonWidth) / 2;
            int button1Y = (int)(0.4f * Screen.height);

            if (GUI.Button(new Rect(buttonX, button1Y, buttonWidth, buttonHeight),
                            message))
                GoToMainMenu();
        }
    }

    private void GoToMainMenu()
    {
        var mapInfosScript = GameObject.Find("Map Infos")
            .GetComponent<MapInfosScript>();

        // We destroy them by hand or else they'll reappear in multi mode
        // \- don't ask me why...
        foreach (GameObject go in mapInfosScript.ListPlayers)
            Destroy(go);

        SceneManager.LoadScene("MainMenu");
    }
}
