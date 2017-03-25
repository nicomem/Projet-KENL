using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkBehaviour
{
    [HideInInspector]
    public MenuMultiplayer networkManagerScriptServer = null;
    [HideInInspector]
    public GameObject persoReady = null;

    void Start()
    {
        networkManagerScriptServer = GameObject.Find("Network Manager")
            .GetComponent<MenuMultiplayer>();

        networkManagerScriptServer.LobbyPlayer = gameObject;
        networkManagerScriptServer.lobbyPlayerScript = this;

        transform.Find("Player Name").GetComponent<Text>().text =
            networkManagerScriptServer.PlayerName;
    }
}
