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

    public void UpdatePersoInSelect()
    {
        if (charaSelected != null) {
            Destroy(charaSelected);
        }

        charaSelected = Instantiate(persoPrefabs[(int)playerSelected]);
        charaSelected.transform.parent = charaSelectBox.transform;
        charaSelected.transform.position = canvas.transform.position;
        charaSelected.transform.localScale = new Vector3(1, 1, 1);

        var playerScript = charaSelected.GetComponent<PlayerScript>();

        // Other fixes for each perso
        switch (playerSelected) {
            case PlayerType.StealthChar:
                charaSelected.transform.localScale =
                    new Vector3(3f, 3f, 3f);
                charaSelected.transform.localPosition +=
                    new Vector3(0, -2.5f, 0);
                charaSelected.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;

            case PlayerType.Antiope:
                charaSelected.transform.localScale =
                    new Vector3(3f, 3f, 3f);
                charaSelected.transform.position +=
                    new Vector3(0, -1f, 0);
                charaSelected.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;

            case PlayerType.VladimirX:
                charaSelected.transform.localScale =
                    new Vector3(2f, 2f, 2f);
                charaSelected.transform.position +=
                    new Vector3(0, -1f, 0);
                charaSelected.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;

            case PlayerType.Satela:
                charaSelected.transform.localScale =
                    new Vector3(3f, 3f, 3f);
                charaSelected.transform.position +=
                    new Vector3(0, -1f, 0);
                charaSelected.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;

            default:
                break;
        }

        charaSelected.transform.localScale *= 0.75f;

        persoName = persosPrefabsNames[(int)playerSelected];

        playerScript.persoName = persoName;
        charaSelectBox.transform.Find("Panel - Perso Name").Find("Perso Name")
            .GetComponent<Text>().text = persoName;
    }

    public void UpdateMapInSelect()
    {
        // Change map in image & sync with clients
        mapSelectBox.transform.Find("Map Name Panel").Find("Map Name")
            .GetComponent<Text>().text = mapScenes[mapSelected];
        mapSelectBox.transform.Find("Map Image").GetComponent<RawImage>().texture =
            mapScreenshots[mapSelected];
    }
}
