using UnityEngine;
using UnityEngine.Networking;

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

    #region SyncVar: attackSelected
    // If -1: no attack selected
    // If 0 <= ... < listAttacks.length: listAttacks[i] selected
    [HideInInspector] [SyncVar] public int attackSelected;
    public void SyncAttackSelected(int i)
    {
        if (isServer || !isNetworked) attackSelected = i;
        else CmdSyncAttackSelected(i);
    }
    [Command] private void CmdSyncAttackSelected(int i)
    {
        attackSelected = i;
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
        SyncAttackSelected(-1);
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

        attackSelected = -1;
        for (int i = 0; i < player.listAttacks.Length; i++) {
            if (Input.GetKeyDown(player.listAttacks[i].inputKey)) {
                attackSelected = i;
                break;
            }
        }

        SyncXInput(xInput);
        SyncJumpButtonPressed(jumpButtonPressed);
        SyncAttackSelected(attackSelected);
    }

    private void MovementPlayer()
    {
        /* To move the player with input */

        player.Movements(xInput, jumpButtonPressed, attackSelected);

        // We make sure there is no movement through Z-Axis
        charaControl.transform.position.Set(
            charaControl.transform.position.x,
            charaControl.transform.position.y,
            0);
    }
}
