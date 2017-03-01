using UnityEngine;
using System;

public class IAScript : MonoBehaviour
{
    private float xInput;
    private bool jumpButtonPressed;
    private bool[] inputs; // true if listAttack[i].inputKey is pressed

    private PlayerScript player;
    private CharacterController charaControl;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerScript>();
        charaControl = GetComponent<CharacterController>();
        
        ColorThePlayers();

        if (transform.name == "Player Human")
        {
            //transform.Rotate(new Vector3(0, 90, 0));
        }

        // All initialized at false by default
        inputs = new bool[player.listAttacks.Length];
    }

    private void Update()
    {
        /* When there's movement or physics, put here */

        if (player.transform.name == "Player 2") // For now we only move player 1
            GetInputs();

        // Function for moving the player with input (!= IA)
        MovementPlayer();
    }

    private void ColorThePlayers()
    {
        /* Add color to players while no 3D models */

        if (transform.name == "Player 2")
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    private void GetInputs()
    {
        inputs[0] = false;
        if (CharaControlScript.player.transform.position.x < player.transform.position.x -2)
            xInput = -1.0f;
        else
            xInput = 1.0f;
        /*else
        {
            System.Random rand = new System.Random();
            if (rand.Next(0, 30) == 1)
                inputs[0] = true;
            else
                xInput = 1.0f;

        }*/
            
        if (player.transform.GetComponent<Renderer>().material.color == Color.gray)
            xInput = +1.0f;
        if ((!CharaControlScript.player.isGrounded && Time.deltaTime > 1.0f) || (CharaControlScript.player.transform.position.y > player.transform.position.y) ) // le time évite le bug au début ou le perso saute sans raison
            jumpButtonPressed = true;
        else
            jumpButtonPressed = false;
        

        /*xInput = Input.GetAxis("Horizontal");
        jumpButtonPressed = Input.GetButtonDown("Jump");

        for (int i = 0; i < player.listAttacks.Length; i++)
        {
            inputs[i] = Input.GetKeyDown(player.listAttacks[i].inputKey);
        }*/

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Reset player1
            player.transform.position = new Vector3(-10, 2.5f, 0);
            player.SetHorizontalVelocity(0f);
            player.SetVerticalVelocity(0f);

            // Reset player2
            GameObject player2 = GameObject.Find("Player 2");
            PlayerScript player2Script = player2.GetComponent<PlayerScript>();

            player2.transform.position = new Vector3(10, 2.5f, 0);
            player2Script.SetHorizontalVelocity(0f);
            player2Script.SetVerticalVelocity(0f);
        }
    }

    private void MovementPlayer()
    {
        /* To move the player with input */

        player.Movements(xInput, jumpButtonPressed, inputs);

        // We make sure there is no movement through Z-Axis
        charaControl.transform.position.Set(
            charaControl.transform.position.x,
            charaControl.transform.position.y,
            0);
    }
}
