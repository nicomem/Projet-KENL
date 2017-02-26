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
            inputs[i] = Input.GetKeyDown(player.listAttacks[i].inputKey);
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
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
        /* To move the player with input (!= IA) */

        player.Movements(xInput, jumpButtonPressed, inputs);

        // We make sure there is no movement through Z-Axis
        charaControl.transform.position.Set(
            charaControl.transform.position.x,
            charaControl.transform.position.y,
            0);
    }
}
