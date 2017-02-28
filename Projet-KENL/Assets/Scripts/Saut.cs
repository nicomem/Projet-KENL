using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saut : MonoBehaviour
{
    public float saut;

        void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) && GetComponent<Rigidbody>().transform.position.y <= 70f)
        {
            Vector3 jump = new Vector3(0.0f, saut, 0.0f);

            GetComponent<Rigidbody>().AddForce(jump);
         }
    }
}