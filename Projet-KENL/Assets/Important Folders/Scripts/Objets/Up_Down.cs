using UnityEngine;

public class Up_Down : MonoBehaviour
{
    public float amplitude = 2;      // Amplitude of movement up/down
    public float speed = 2;          // Speed up/down
    public float rotationSpeed = 90; // Rotation in 1 second

    private float tempVal;
    private Vector3 tempPos;

    void Start()
    {
        tempVal = transform.position.y;
        tempPos = transform.position;
    }

    void Update()
    {
        tempPos.y = tempVal + amplitude * Mathf.Sin(speed * Time.time);
        transform.position = tempPos;

        transform.Rotate(transform.InverseTransformVector(Vector3.up),
            rotationSpeed * Time.deltaTime);
    }
}