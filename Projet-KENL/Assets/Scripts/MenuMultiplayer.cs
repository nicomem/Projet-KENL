using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuMultiplayer : NetworkManager
{
    public Text inputFieldIPAddressText;
    // public Scene backScene;
    // public Scene[] levelScenes;

    public void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    private void SetIPAddress()
    {
        string ipAddress = inputFieldIPAddressText.text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    private void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    public void Load_MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}