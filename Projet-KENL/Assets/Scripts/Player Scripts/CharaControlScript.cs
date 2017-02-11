using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharaControlScript : MonoBehaviour
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

        // All initialized at false by default
        inputs = new bool[player.listAttacks.Length];
    }

    private void Update()
    {
        /* When there's movement or physics, put here */

        if (transform.name == "Player") // For now we only move player 1
            GetInputs();

        // Function for moving the player with input (!= IA)
        MovementPlayer();
    }

    private void ColorThePlayers()
    {
        /* Add color to players while no 3D models */

        if (transform.name == "Player") {
            GetComponent<Renderer>().material.color = Color.green;
        } else {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    private void GetInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        jumpButtonPressed = Input.GetButtonDown("Jump");

        for (int i = 0; i < player.listAttacks.Length; i++) {
            inputs[i] = Input.GetKeyDown(player.listAttacks[i].InputKey);
        }

        if (Input.GetKeyDown(KeyCode.Return))
            player.transform.position = new Vector3(-10, 2.5f, 0);
    }

    private void MovementPlayer()
    {
        /* To move the player with input (!= IA) */

        player.Movements(xInput, jumpButtonPressed, inputs);

        // We make sure there is no movement through Z-Axis
        charaControl.transform.position.Set(
            charaControl.transform.position.x,
            charaControl.transform.position.y,
            0);
    }
}
