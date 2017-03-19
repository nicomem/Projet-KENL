using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkBehaviour {
    public MenuMultiplayer networkManagerScript;
    public GameObject persoReady = null;

    void Start () {
        networkManagerScript = GameObject.Find("Network Manager")
            .GetComponent<MenuMultiplayer>();

        networkManagerScript.LobbyPlayer = gameObject;
        networkManagerScript.lobbyPlayerScript = this;

        transform.Find("Player Name").GetComponent<Text>().text =
            networkManagerScript.PlayerName;
    }
}
