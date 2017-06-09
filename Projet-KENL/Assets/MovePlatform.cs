using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour {

    [SerializeField]
    Transform Platform;

    [SerializeField]
    Transform StartTransform;

    [SerializeField]
    Transform EndTransform;

    [SerializeField]
    float PlatformSpeed;

    Vector3 direction;
    Transform destination;

    private Rigidbody rb;

    void Start()
    {
        SetDestination(StartTransform);
    }

    void FixedUpdate()
    {
        //Platform.rigidbody.MovePosition(Platform.position * Vector3.right * PlatformSpeed * Time.fixedDeltaTime);
        rb = this.Platform.GetComponent<Rigidbody>();
        rb.MovePosition(Platform.position + direction * PlatformSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(Platform.position, destination.position) < PlatformSpeed * Time.fixedDeltaTime)
        {
            SetDestination(destination == StartTransform ? EndTransform : StartTransform);
        }

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(StartTransform.position, Platform.localScale);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(EndTransform.position, Platform.localScale);

    }

    void SetDestination(Transform dest)
    {
        destination = dest;
        direction = (destination.position = Platform.position).normalized;
    }
}
