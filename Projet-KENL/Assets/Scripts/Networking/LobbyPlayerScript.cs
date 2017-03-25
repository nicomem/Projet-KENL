using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkBehaviour
{
    [HideInInspector]
    public MenuMultiplayer networkManagerScript = null;
    [HideInInspector]
    public GameObject persoReady = null;

    private NetworkConnection localPlayerConn;
    [SyncVar]
    private int indexPlayer = -1;
    private bool positioned;

    void Start()
    {
        positioned = false;

        transform.SetParent(GameObject.Find("Canvas").transform);
        // Make it "invisible" while not positioned
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    public override void OnStartAuthority()
    {
        // This does not work!
        localPlayerConn = connectionToClient;
        CmdGetIndexPlayer();

        Debug.Log(connectionToServer);
        Debug.Log(connectionToClient);

        networkManagerScript = GameObject.Find("Network Manager")
            .GetComponent<MenuMultiplayer>();

        networkManagerScript.LobbyPlayer = gameObject;
        networkManagerScript.lobbyPlayerScript = this;

        transform.Find("Player Name").GetComponent<Text>().text =
            networkManagerScript.PlayerName;
    }

    private void Update()
    {
        if (!positioned && indexPlayer != -1) {
            int x, y;
            x = 520 * (indexPlayer % 2 == 0 ? -1 : 1);
            y = 180 * (indexPlayer >= 2 ? -1 : 1);

            transform.localPosition = new Vector3(x, y, 0);
            transform.localScale = new Vector3(1, 1, 1);
            transform.gameObject.SetActive(true);

            positioned = true;
        }
    }

    [Command]
    public void CmdGetIndexPlayer()
    {
        var listPlayers = GameObject.Find("Network Manager")
                .GetComponent<MenuMultiplayer>().listPlayers;

        for (short i = 0; i < listPlayers.Length; i++) {
            if (listPlayers[i] == localPlayerConn) {
                indexPlayer = i;
                break;
            }
        }

        if (indexPlayer == -1) {
            Debug.Log("LobbyPlayerScript: CmdGetIndexPlayer: indexPlayer == -1");
        }
    }
}
