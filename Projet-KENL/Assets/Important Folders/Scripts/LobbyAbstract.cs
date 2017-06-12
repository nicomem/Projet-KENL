using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyAbstract : NetworkManager
{
    protected GameObject canvas;
    protected GameObject charaSelectBox;
    protected GameObject readyButton;
    protected GameObject mapSelectBox;
    protected GameObject chooseMapButton;

    public GameObject[] persoPrefabs;
    public string[] persosPrefabsNames;
    public string[] mapScenes;
    public Texture[] mapScreenshots;

    protected string persoName;
    protected GameObject charaSelected;
    protected PlayerType playerSelected = 0;
    protected int mapSelected = 0;

    public enum PlayerType { StealthChar, Antiope, VladimirX, Satela };

    public void UpdateMapInSelect()
    {
        // Change map in image & sync with clients
        mapSelectBox.transform.Find("Map Name Panel").Find("Map Name")
            .GetComponent<Text>().text = mapScenes[mapSelected];
        mapSelectBox.transform.Find("Map Image").GetComponent<RawImage>().texture =
            mapScreenshots[mapSelected];
    }
}
