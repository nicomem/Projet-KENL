using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
    [SyncVar] public int indexPlayer;

    void Start()
    {
        if (SceneManager.GetActiveScene() !=
            SceneManager.GetSceneByName("MultiplayerLobby")) {
            Destroy(gameObject);
            return;
        }
            
        var canvas = GameObject.Find("Canvas").transform;
        transform.SetParent(canvas, false);
        transform.localScale = new Vector3(1, 1, 1);

        PlayerNameText = transform.Find("Player Name").GetComponent<Text>();
        PersoNameText = transform.Find("Perso Name").GetComponent<Text>();
        IsReadyText = transform.Find("IsReady Text").GetComponent<Text>();

        PersoNameText.text = "";
        if (transform.localPosition.x != 0)
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
        transform.localPosition = position;
    }

    [Command] public void CmdTellReady(bool isReady)
    {
        GameObject.Find("Network Manager").GetComponent<MenuMultiplayer>()
            .UpdateReady(indexPlayer, isReady);
    }

    [Command] public void CmdDisconnect()
    {
        RpcDisconnect();
    }

    [ClientRpc] public void RpcDisconnect()
    {
        Destroy(gameObject);
    }

    #region SyncVar (because [SyncVar] only Server -> Client)
    [Command] public void CmdSyncPlayerName(string playerName)
    {
        this.playerName = playerName;
    }

    [Command] public void CmdSyncPersoName(string persoName, int indexPlayer)
    {
        this.persoName = persoName;

        var ServerManagerScript = GameObject.Find("Network Manager")
            .GetComponent<MenuMultiplayer>();
        ServerManagerScript.persoNames[indexPlayer] = persoName;
    }

    [Command] public void CmdSyncIsReady(bool isReady)
    {
        this.isReady = isReady;
    }

    [Command] public void CmdSyncIndexPlayer(short indexPlayer)
    {
        this.indexPlayer = indexPlayer;
    }

    [Command] public void CmdSyncPosition(Vector3 position)
    {
        this.position = position;
    }
    #endregion
}
