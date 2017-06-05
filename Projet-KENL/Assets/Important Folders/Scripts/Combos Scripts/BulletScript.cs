using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 1.0f;
    public float dirX = 0f;
    public PlayerScript player;

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

    private void GiveAttack(PlayerScript other)
    {
        Debug.Log(player.persoName + " is hitting on " + other.persoName);
    }
}
