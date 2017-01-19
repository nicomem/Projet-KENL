using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour {
    public float gravity = 14.0f;
    public float jumpForce = 10.0f; // Y-velocity added when jumping
    public float horizontalVelocity = 20.0f;
    public int jumpMax = 2; // How many jumps the player can do
                            // before being grounded

    private CharacterController controller;
    private float verticalVelocity;
    private float xInput, yInput;
    private int jumpCount = 0; // How many jumps done before grounded

    // Add color to players while no 3D models
    private void ColorThePlayers() {
        if (transform.name == "Player 1") {
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
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        if (transform.name != "Player 1")
            return;

        if (controller.isGrounded) {
            jumpCount = 0;

            if (yInput > 0.1f) {
                verticalVelocity = jumpForce;
                jumpCount++;
            }
        } else {
            if (jumpCount < jumpMax
                    && verticalVelocity < 0.1 * jumpForce
                    && yInput > 0.25f) {
                verticalVelocity = jumpForce;
                jumpCount++;
            } else if (verticalVelocity > 0 && yInput <= 0.1f) {
                verticalVelocity *= 0.5f;
            }
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 moveVector = Vector3.zero;
        float waySign = IsCorrectWay() ? 1 : -1;

        moveVector.x = xInput * horizontalVelocity;
        moveVector.y = verticalVelocity;
        moveVector.z = 0;
        controller.Move(moveVector * Time.deltaTime);

        // Rotate around Y-Axis if player goes backwards
        if (waySign * xInput < 0) {
            transform.Rotate(new Vector3(0, waySign * 180));
        }
    }

    private bool IsCorrectWay() {
        /* Returns true if the player is facing to the right */
        return controller.transform.eulerAngles.y == 0;
    }
}
