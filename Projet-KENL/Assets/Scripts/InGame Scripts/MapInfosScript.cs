using UnityEngine;
using System.Linq;

public class MapInfosScript : MonoBehaviour
{
    [HideInInspector] public GameObject[] listPlayers;
    private bool[] playersInitiated;

    public float xMinLimit, xMaxLimit, yMinLimit, yMaxLimit;
    private float currentY, currentX;
    public bool initPlayersFinished = false;

    private void Start()
    {
        listPlayers = GameObject.FindGameObjectsWithTag("Player");
        playersInitiated = new bool[listPlayers.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (!initPlayersFinished)
            InitPlayers();
        else {
            CheckEjected();
        }
    }

    private void CheckEjected()
    {
        for (short i = 0; i < listPlayers.Length; i++) {
            currentY = listPlayers[i].transform.position.y;
            currentX = listPlayers[i].transform.position.x;

            if (currentY < yMinLimit || currentY > yMaxLimit
             || currentX < xMinLimit || currentX > xMaxLimit) {
                /* Animation ejected + remove player */
                /* If --player.lives > 0 => respawn player */
            }
        }
    }

    private void InitPlayers()
    {
        for (ushort i = 0; i < listPlayers.Length; i++) {
            var player = listPlayers[i];
            var playerScript = player.GetComponent<PlayerScript>();

            if (!playersInitiated[i] && playerScript.persoName != null
                && playerScript.persoName != "") {
                switch (playerScript.persoName) {
                    case "Stealth Char":
                        // Set Rotate90 as parent (& do things)
                        var go = new GameObject("Stealth Char - Rotate90");
                        go.transform.position = Vector3.zero;
                        DontDestroyOnLoad(go);
                        player.transform.localPosition = new Vector3(0, 1.5f, 0);
                        player.transform.localRotation = Quaternion.identity;
                        player.transform.SetParent(go.transform, true);
                        go.transform.rotation = Quaternion.Euler(0, 90, 0);
                        break;

                    case "Player Test":
                        break;
                }

                if (playerScript.isIA)
                    player.AddComponent<IAScript>();
                else
                    player.AddComponent<CharaControlScript>();

                playersInitiated[i] = true;
            }
        }

        initPlayersFinished = playersInitiated.All(b => b);
    }
}
