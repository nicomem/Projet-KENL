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

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: If collide with other player
    }
}
