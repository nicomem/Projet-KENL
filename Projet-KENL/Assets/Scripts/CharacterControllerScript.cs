using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour {
    public float gravity = 14.0f;
    public float jumpForce = 10.0f; // Y-velocity added when jumping
    public float horizontalVelocity = 20.0f;

    private CharacterController controller;
    private float verticalVelocity;

    // Add color to players while no 3D models
    private void ColorThePlayers() {
        if(transform.name == "Player 1") {
            GetComponent<Renderer>().material.color = Color.green;
        } else {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    // Use this for initialization
    void Start() {
        controller = GetComponent<CharacterController>();
        ColorThePlayers();
    }

    // Update is called once per period (fixed time)
    private void FixedUpdate() {
        if (transform.name != "Player 1")
            return;

        if (controller.isGrounded) {
            if (Input.GetAxis("Vertical") > 0.1f) {
                verticalVelocity = jumpForce;
            }
        } else {
            if (verticalVelocity > 0 && Input.GetAxis("Vertical") <= 0.1f) {
                verticalVelocity *= 0.5f;
            }
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 moveVector = Vector3.zero;
        moveVector.x = Input.GetAxis("Horizontal") * horizontalVelocity;
        moveVector.y = verticalVelocity;
        controller.Move(moveVector * Time.deltaTime);
    }
}
