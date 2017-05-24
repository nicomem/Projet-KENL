using UnityEngine;

public class BlockScript : MonoBehaviour
{
    // Script used for blocking (defense, shield, you see)

    public string inputKey = "left ctrl";
    public GameObject shieldGO;
    private PlayerScript player;

    private void Start()
    {
        player = GetComponent<PlayerScript>();
    }

    public void Block()
    {
        // Begin (or continue) blocking

        player.SyncIsBlocking(true);
        shieldGO.SetActive(true);

        shieldGO.transform.rotation = Quaternion.identity;
    }

    public void StopBlocking()
    {
        // Stop blocking

        player.SyncIsBlocking(false);
        shieldGO.SetActive(false);
    }
}
