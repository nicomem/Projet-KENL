using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 1.0f;
    public float dirX = 0f;
    public PlayerScript player;

    private void Update()
    {
        transform.Translate(transform.InverseTransformVector(
            new Vector3(dirX * speed, 0, 0)));

        // TODO: Destroy if outside limits
    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO: If collide with other player
    }
}
