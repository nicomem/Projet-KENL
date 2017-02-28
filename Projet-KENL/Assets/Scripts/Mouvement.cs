using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement : MonoBehaviour {

    public float vitesse;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    } 

    void FixedUpdate()
    {
        float mouvement = Input.GetAxis("Horizontal");

        Vector2 move = new Vector2(mouvement, 0.0f);

        rb.AddForce (move * vitesse);
    }
}
