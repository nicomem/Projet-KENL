using UnityEngine;

public class Up_Down : MonoBehaviour
{
    public float amplitude = 2;          //Set in Inspector 
    public float speed = 2;                  //Set in Inspector 
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

        // Rotations "spéciales"
        //transform.Rotate(transform.TransformVector(Vector3.up),
        //    rotationSpeed * Time.deltaTime);

        transform.Rotate(transform.InverseTransformVector(Vector3.up),
            rotationSpeed * Time.deltaTime);
    }
}