using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    public float gravity = 14.0f;
    public float jumpForce = 10.0f; // Y-velocity added when jumping
    public float horizontalVelocity = 20.0f;
    public int jumpMax = 2; // How many jumps the player can do
                            // before being grounded

    public Attack[] listAttacks;

    private float verticalVelocity;
    private float xInput, yInput;
    private float waySign; // If 1: player looks to the right
    private int jumpCount = 0; // How many jumps done before grounded
    // powerReceived = power * (1 + percentHealth / 100)
    private float percentHealth = 0;
    private bool isAttacking = false; // See if the player is attacking

    private CharacterController controller;
    private Vector3 moveVector;

    [System.Serializable]
    public class Attack
    {
        public float power;
        public Collider collider;
        public string inputKey; // Input.GetKeyDown(inputKey)

        public Attack(float _power, Collider _collider, string _inputKey)
        {
            power = _power;
            collider = _collider;
            inputKey = _inputKey;
        }
    }

    // Add color to players while no 3D models
    private void ColorThePlayers()
    {
        if (transform.name == "Player 1")
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        ColorThePlayers();
    }

    private void FixedUpdate()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        if (transform.name != "Player 1") // For now we only move player 1
            return;

        // Call all movements functions here
        MoveJump(yInput);
        MoveGravity();
        isAttacking = MoveAttack();

        moveVector = Vector3.zero;
        waySign = IsCorrectWay() ? 1 : -1;

        moveVector.x = xInput * horizontalVelocity;
        moveVector.y = verticalVelocity;
        // We correct the position if z != 0
        moveVector.z = -transform.position.z / Time.deltaTime;
        controller.Move(moveVector * Time.deltaTime);

        // Rotate around Y-Axis if player goes backwards
        if (waySign * xInput < 0)
        {
            transform.Rotate(new Vector3(0, waySign * 180));
        }
    }

    private bool IsCorrectWay()
    {
        /* Returns true if the player is facing to the right */
        return controller.transform.eulerAngles.y == 0;
    }

    private bool MoveJump(float yInput)
    {
        /* Verify if a jump can be made, if so, makes the player jump
         * Returns a bool indicating if a jump has been made
         */

        if (controller.isGrounded)
        {
            jumpCount = 0;
            verticalVelocity = 0;
        }
        else if (jumpCount == 0)
        {
            // If we fall without making a jump, we lose 1 jump
            jumpCount = 1;
        }

        if (jumpCount < jumpMax
            && verticalVelocity < 0.1 * jumpForce
            && yInput > 0.25f)
        {
            verticalVelocity = jumpForce;
            jumpCount++;
        }
        else if (verticalVelocity > 0 && yInput <= 0.1f)
        {
            verticalVelocity *= 0.5f;
        }

        return false;
    }

    private bool MoveGravity()
    {
        if (!controller.isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            return true;
        }
        return false;
    }

    private bool MoveAttack()
    {
        if (isAttacking)
        {
            // TODO Insert a timer here
            return false;
        }

        foreach (Attack attack in listAttacks)
        {
            if (Input.GetKeyDown(attack.inputKey))
            {
                LaunchAttack(attack.collider);
            }
        }

        return false;
    }

    private void LaunchAttack(Collider collider)
    {
        Collider[] colliders =
            Physics.OverlapBox(collider.bounds.center,
                               collider.bounds.extents,
                               collider.transform.rotation,
                               LayerMask.GetMask("Hitbox"));

        foreach (Collider col in colliders)
        {
            if (col.transform.name != transform.name) // No self-hitting here !
            {
                print(col.transform.name);
            }
        }
    }
}
