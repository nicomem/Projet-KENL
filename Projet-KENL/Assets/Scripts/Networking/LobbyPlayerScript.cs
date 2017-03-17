using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerScript : MonoBehaviour {

    void Start () {
        var networkManagerScript = GameObject.Find("Network Manager")
            .GetComponent<MenuMultiplayer>();

        networkManagerScript.LobbyPlayer = gameObject;

        transform.Find("Player Name").GetComponent<Text>().text =
            networkManagerScript.PlayerName;
    }
}
