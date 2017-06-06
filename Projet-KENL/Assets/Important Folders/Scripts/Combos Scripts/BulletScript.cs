using UnityEngine;
using UnityEngine.Networking;

public class BulletScript : NetworkBehaviour
{
    public float speed = 1f;
    public float dirX = 0f;
    public float power = 2f;
    public float multPush = 4f;

    [HideInInspector] public PlayerScript player;
    private MapInfosScript mapInfos;

    private void Start()
    {
        mapInfos = GameObject.Find("Map Infos").GetComponent<MapInfosScript>();
    }

    private void Update()
    {
        transform.Translate(transform.InverseTransformVector(
            new Vector3(dirX * speed, 0, 0)));

        // Destroy if outside limits
        if (transform.position.x > mapInfos.xMaxLimit
            || transform.position.x < mapInfos.xMinLimit
            || transform.position.y > mapInfos.yMaxLimit
            || transform.position.y < mapInfos.yMinLimit)
            Destroy(gameObject);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        // No self-hitting
        if (other.transform.GetInstanceID() == player.transform.GetInstanceID())
            return;

        // Destroy when hitting a plateform
        if (other.transform.CompareTag("Plateform")) {
            Destroy(gameObject);
            return;
        }

        // Do attacking & stuffs
        if (other.transform.CompareTag("Player")) {
            GiveAttack(other.transform.GetComponent<PlayerScript>());
            // Destroy when 1 hit or continue ?
            // return;
        }
    }
    
    private void GiveAttack(PlayerScript playerHit)
    {
        float healthMultiplier = 1 + (playerHit.percentHealth / 100);
        float leftRight;
        if (transform.position.x < playerHit.transform.position.x)
            leftRight = 1f;
        else
            leftRight = -1f;

        float x = power * leftRight * healthMultiplier
            * multPush;
        float y = power * healthMultiplier * multPush;

        // Place it in mid-air (== not grounded)
        playerHit.AddPosY(0.1f);
        playerHit.AddVelocities(x, y);
        playerHit.SyncInvulnerableTimer(0.5f);
        playerHit.SyncPercentHealth(playerHit.percentHealth + power);
    }
}
