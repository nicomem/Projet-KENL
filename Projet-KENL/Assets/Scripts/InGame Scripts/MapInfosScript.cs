using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapInfosScript : MonoBehaviour
{
    public GameObject[] ListPlayers { get; private set; }
    public PlayerScript[] ListPlayerScripts { get; private set; }
    private bool[] playersInitiated;
    public Vector3[] startPositions;
    public Vector3[] respawnPositions;

    public float xMinLimit, xMaxLimit, yMinLimit, yMaxLimit;
    private float currentY, currentX;
    public bool InitPlayersFinished { get; private set; }

    private void Start()
    {
        ListPlayers = GameObject.FindGameObjectsWithTag("Player");

        ListPlayerScripts = new PlayerScript[ListPlayers.Length];
        for (short i = 0; i < ListPlayers.Length; i++)
            ListPlayerScripts[i] = ListPlayers[i].GetComponent<PlayerScript>();

        playersInitiated = new bool[ListPlayers.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (!InitPlayersFinished)
            InitPlayers();
        else {
            CheckEjected();
            //CheckVictory();
        }
    }

    private void InitPlayers()
    {
        for (ushort i = 0; i < ListPlayers.Length; i++) {
            var player = ListPlayers[i];
            var playerScript = ListPlayerScripts[i];
            var startPos = startPositions[i];

            if (!playersInitiated[i] && playerScript.persoName != null
                && playerScript.persoName != "") {
                player.transform.SetParent(null);

                switch (playerScript.persoName) {
                    case "Stealth Char":
                        // Set Rotate90 as parent (& do things)
                        var parent = new GameObject("Stealth Char - Rotate90");
                        parent.transform.position = Vector3.zero;
                        parent.transform.rotation = Quaternion.Euler(0, 90, 0);

                        player.transform.SetParent(parent.transform);
                        player.transform.localRotation = Quaternion.identity;
                        break;

                    case "Player Test":
                        break;

                    default:
                        Debug.Log("[ERR] MapInfosScript/InitPlayers: " +
                            "persoName not recognized : " + playerScript.persoName);
                        break;
                }

                player.transform.position = startPos;

                if (playerScript.isIA)
                    player.AddComponent<IAScript>();
                else
                    player.AddComponent<CharaControlScript>();

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
            currentY = player.transform.position.y;
            currentX = player.transform.position.x;

            if (currentY < yMinLimit || currentY > yMaxLimit
             || currentX < xMinLimit || currentX > xMaxLimit) {
                Debug.Log(currentX + " " + currentY);
                var playerScript = player.GetComponent<PlayerScript>();

                playerScript.CmdSyncPersoLives(playerScript.persoLives - 1);

                if (playerScript.persoLives <= 0) {
                    playerScript.CmdSyncIsKO(true);
                    // Do not destroy player but disable it
                    // So we can still get its infos
                    player.SetActive(false);
                } else {
                    /* Animation ejected */
                    player.transform.position = respawnPositions[i];
                }
            }
        }
    }

    private void CheckVictory()
    {
        List<PlayerScript> activePlayerScripts = new List<PlayerScript>();
        foreach (var playerScript in ListPlayerScripts) {
            if (!playerScript.isKO)
                activePlayerScripts.Add(playerScript);
        }

        if (activePlayerScripts.Count == 1)
            Debug.Log("[INF] Victory: " + activePlayerScripts[0].playerName);
        else if (activePlayerScripts.Count == 0)
            Debug.Log("[INF] It's a tie");
    }
}
