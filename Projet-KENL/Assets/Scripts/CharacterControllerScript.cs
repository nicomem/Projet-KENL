using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerScript : MonoBehaviour
{
    // Jump var (public)
    public float gravity = 14.0f;
    public float jumpForce = 10.0f; // Y-velocity added when jumping
    public float horizontalVelocity = 20.0f;
    public int jumpMax = 2; // How many jumps the player can do
                            // before being grounded
    
    
    // Attack var (public)
    public Attack[] listAttacks;
    public int maxCombo = 4;


    // Jump var (private)
    private float verticalVelocity;
    private int jumpCount = 0; // How many jumps done before grounded

    // Attack var (private)
    private float percentHealth = 0; // powerReceived = 
                                     // power * (1 + percentHealth / 100)
    private bool isAttacking = false; // See if the player is attacking
    private float attackTimer = 0f; // If 0f, the player can attack
                                    // again (no combos)
    private int comboActual = 0;


    // Other movements var (private)
    private float xInput, yInput;
    private bool[] inputs; // true if listAttack[i].inputKey is pressed
    private float waySign; // If 1: player looks to the right

    private CharacterController controller;
    private Vector3 moveVector;


    // Attack class
    [System.Serializable]
    public class Attack
    {
        public float power;
        public Collider collider;
        public string inputKey; // Input.GetKeyDown(inputKey)
        public float attackCooldown; // The time to end the attack (s)
        public int comboIncrease;

        public Attack(float _power, 
            Collider _collider, 
            string _inputKey, 
            float _attackCooldown = 1, 
            int _comboIncrease = 1)
        {
            power = _power;
            collider = _collider;
            inputKey = _inputKey;
            attackCooldown = _attackCooldown;
            comboIncrease = _comboIncrease;
        }
    }


    // Add color to players while no 3D models
    private void ColorThePlayers()
    {
        if (transform.name == "Player")
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();

        // All initialized at false by default
        inputs = new bool[listAttacks.Length];

        ColorThePlayers();
    }

    private void Update()
    {
        if (transform.name != "Player") // For now we only move player 1
            return;

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        for (int i = 0; i < listAttacks.Length; i++)
        {
            inputs[i] = Input.GetKeyDown(listAttacks[i].inputKey);
        }

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
        if (attackTimer > 0f) { attackTimer -= Time.deltaTime; }

        if (attackTimer > 0)
        {
            
            // Give a color to the collider (DEBUG)
            transform.GetChild(1).GetComponent<Renderer>()
                .material.color = Color.black;

            if (attackTimer < 0.5f)
            {
                transform.GetChild(1).GetComponent<Renderer>()
                .material.color = Color.yellow;
            }
        }
        else
        {
            // Remove color of the collider (DEBUG)
            transform.GetChild(1).GetComponent<Renderer>()
                .material.color = Color.white;

            comboActual = 0; // We don't forget to reset the combo var
        }

        if (comboActual < maxCombo && attackTimer < 0.5f)
        {
            for (int i = 0; i < listAttacks.Length; i++)
            {
                if (inputs[i]) // If attack button pressed
                {
                    LaunchAttack(listAttacks[i].collider, listAttacks[i]);
                    comboActual += listAttacks[i].comboIncrease;
                    attackTimer = listAttacks[i].attackCooldown;

                    return true;
                }
            }
        }

        return false;
    }

    private void LaunchAttack(Collider collider, Attack attack)
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

    public void AddMovement(Vector3 movement)
    {
        /* Call this function to add a movement
         * to this player (from another player) */
        moveVector += movement;
    }
}
