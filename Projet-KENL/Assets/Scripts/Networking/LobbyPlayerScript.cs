using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LobbyPlayerScript : NetworkBehaviour
{
    [HideInInspector] public MenuMultiplayer networkManagerScript = null;

    private Text PlayerNameText;
    private Text PersoNameText;
    private Text IsReadyText;

    [SyncVar] public string playerName;
    [SyncVar] public string persoName;
    [SyncVar] public bool isReady = false;

    [SyncVar] private Vector3 position;

    void Start()
    {
        var canvas = GameObject.Find("Canvas").transform;
        transform.SetParent(canvas, false);
        transform.localScale = new Vector3(1, 1, 1);

        PlayerNameText = transform.Find("Player Name").GetComponent<Text>();
        PersoNameText = transform.Find("Perso Name").GetComponent<Text>();
        IsReadyText = transform.Find("IsReady Text").GetComponent<Text>();

        PersoNameText.text = "";
        CmdSyncPosition(transform.localPosition);
    }

    public override void OnStartAuthority()
    {
        networkManagerScript = GameObject.Find("Network Manager")
            .GetComponent<MenuMultiplayer>();

        networkManagerScript.LobbyPlayer = gameObject;
        networkManagerScript.lobbyPlayerScript = this;
        CmdSyncPlayerName(networkManagerScript.PlayerName);
    }

    private void Update()
    {
        if (playerName != null) PlayerNameText.text = playerName;
        if (persoName != null) PersoNameText.text = persoName;
        if (isReady) IsReadyText.text = "Ready!";
            else IsReadyText.text = "Not Ready";
        if (position != null) transform.localPosition = position;
    }

    #region SyncVar (because [SyncVar] only Server -> Client)
    [Command]
    public void CmdSyncPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    [Command]
    public void CmdSyncPersoName(string persoName)
    {
        Debug.Log("Perso Name: " + persoName);
        this.persoName = persoName;
    }

    [Command]
    public void CmdSyncIsReady(bool isReady)
    {
        this.isReady = isReady;
    }

    [Command]
    public void CmdSyncPosition(Vector3 position)
    {
        if (this.position != null)
            this.position = position;
    }
    #endregion
}
