using UnityEngine;
using UnityEngine.Networking;

public class CharaControlScript : NetworkBehaviour
{
    private bool isNetworked; // is in multi mode or single mode ?

    private float xInput;
    private int attackSelected;
    private bool jumpButtonPressed, blockPressed;

    private PlayerScript player;
    private CharacterController charaControl;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerScript>();
        charaControl = GetComponent<CharacterController>();

        isNetworked = GameObject.Find("Network Manager") != null;
    }

    private void Update()
    {
        if (isNetworked && !hasAuthority)
            return;

        GetInputs();
        MovementPlayer();
    }

    private void GetInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        jumpButtonPressed = Input.GetButtonDown("Jump");

        attackSelected = -1;
        for (int i = 0; i < player.listAttacks.Length; i++) {
            if (Input.GetKeyDown(player.listAttacks[i].inputKey)) {
                attackSelected = i;
                break;
            }
        }

        blockPressed = Input.GetKey(player.blockScript.inputKey);
    }

    private void MovementPlayer()
    {
        /* To move the player with input */

        player.CmdMovements(xInput, jumpButtonPressed, attackSelected,
            blockPressed);

        // We make sure there is no movement through Z-Axis
        charaControl.transform.position.Set(
            charaControl.transform.position.x,
            charaControl.transform.position.y,
            0);
    }
}
