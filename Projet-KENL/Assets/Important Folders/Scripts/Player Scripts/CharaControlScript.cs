using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class CharaControlScript : NetworkBehaviour
{
    #region SyncVar region
    #region SyncVar: xInput
    [HideInInspector] [SyncVar] public float xInput;
    public void SyncXInput(float f)
    {
        if (isServer || !isNetworked) xInput = f;
        else CmdSyncXInput(f);
    }
    [Command] private void CmdSyncXInput(float f) { xInput = f; }
    #endregion

    #region SyncVar: jumpButtonPressed
    [HideInInspector] [SyncVar] public bool jumpButtonPressed;
    public void SyncJumpButtonPressed(bool b)
    {
        if (isServer || !isNetworked) jumpButtonPressed = b;
        else CmdSyncJumpButtonPressed(b);
    }
    [Command] private void CmdSyncJumpButtonPressed(bool b) { jumpButtonPressed = b; }
    #endregion

    #region SyncVar: attackInputs
    // true if listAttack[i].inputKey is pressed
    [HideInInspector] [SyncVar] public SyncListBool attackInputs = new SyncListBool();
    public void SyncAttackInputs(SyncListBool bList)
    {
        if (isServer || !isNetworked) attackInputs = bList;
        else {
            bool[] bArray = new bool[bList.Count];
            bList.CopyTo(bArray, 0);
            CmdSyncAttackInputs(bArray);
        }
    }
    [Command] private void CmdSyncAttackInputs(bool[] bArray)
    {
        for (int i = 0; i < bArray.Length; i++)
            attackInputs[i] = bArray[i];
    }
    #endregion
    #endregion

    //private float xInput;
    //private bool jumpButtonPressed;
    //private bool[] inputs; // true if listAttack[i].inputKey is pressed
    private bool isNetworked; // is in multi mode or single mode ?

    private PlayerScript player;
    private CharacterController charaControl;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerScript>();
        charaControl = GetComponent<CharacterController>();

        isNetworked = GameObject.Find("Network Manager") != null;

        // All initialized at false by default
        for (int i = 0; i < player.listAttacks.Length; i++)
            attackInputs.Add(false);
    }

    private void Update()
    {
        if (hasAuthority || !isNetworked)
            GetInputs();

        MovementPlayer();
    }

    private void GetInputs()
    {
        xInput = Input.GetAxis("Horizontal");
        jumpButtonPressed = Input.GetButtonDown("Jump");

        for (int i = 0; i < player.listAttacks.Length; i++) {
            attackInputs[i] = Input.GetKeyDown(player.listAttacks[i].inputKey);
        }

        SyncXInput(xInput);
        SyncJumpButtonPressed(jumpButtonPressed);
        SyncAttackInputs(attackInputs);
    }

    private void MovementPlayer()
    {
        /* To move the player with input */

        player.Movements(xInput, jumpButtonPressed, attackInputs);

        // We make sure there is no movement through Z-Axis
        charaControl.transform.position.Set(
            charaControl.transform.position.x,
            charaControl.transform.position.y,
            0);
    }
}
