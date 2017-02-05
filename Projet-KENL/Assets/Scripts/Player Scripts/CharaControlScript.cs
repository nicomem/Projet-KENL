using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharaControlScript : MonoBehaviour
{
    private float xInput, yInput;
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

    private void MovementPlayer()
    {
        /* For move the player with input (!= IA) */

        if (transform.name != "Player") // For now we only move player 1
            return;

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        for (int i = 0; i < player.listAttacks.Length; i++) {
            inputs[i] = Input.GetKeyDown(player.listAttacks[i].inputKey);
        }

        player.Movements(xInput, yInput, inputs);
        charaControl.Move(player.GetMoveVector() * Time.deltaTime);

        // We make sure there is no movement through Z-Axis
        charaControl.transform.position.Set(
            charaControl.transform.position.x,
            charaControl.transform.position.y,
            0);
    }
}
