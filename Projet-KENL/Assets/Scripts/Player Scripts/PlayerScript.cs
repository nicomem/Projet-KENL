using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerScript : NetworkBehaviour
{
    [HideInInspector] [SyncVar] public string persoName;
    [Command] public void CmdSyncPersoName(string s) { persoName = s; }

    [HideInInspector] [SyncVar] public bool isIA;
    [Command] public void CmdSyncIsIA(bool b) { isIA = b; }

    [HideInInspector] [SyncVar] public Vector3 syncPos;
    [Command] public void CmdSyncPosXY(Vector3 p) { syncPos = p; }

    [HideInInspector] [SyncVar] public Quaternion syncRotation;
    [Command] public void CmdSyncRotation(Quaternion r) { syncRotation = r; }

    [HideInInspector] [SyncVar] public bool isGrounded = true;
    [Command] public void CmdSyncIsGrounded(bool b) { isGrounded = b; }

    // If <= 0f, player can get hit, resets in function of the attack
    [HideInInspector] [SyncVar] public float InvulnerableTimer;
    [Command] public void CmdSyncInvulnerableTimer(float t) { InvulnerableTimer = t; }

    [HideInInspector] [SyncVar] public float percentHealth = 0;
    [Command] public void CmdSyncPercentHealth(float h) { percentHealth = h; }

    // Jump var (Editor)
    [Header("Jumping")]
    [Space(5)]
    public float gravity = 60f;
    public float jumpForce = 30f; // Y-velocity added when jumping
    public float horizontalSpeed = 20f;
    public int jumpMax = 2; // How many jumps the player can do
                            // before being grounded
    public Collider jumpCollider;

    [Space]

    // Attack var (Editor)
    [Header("Attacking")]
    [Space(5)]
    public ComboTemplate[] listAttacks;
    public int maxCombo = 4;
    public float period = 1f; // Set the period between each attackCollider check

    [Space]
    public AnimationsScript animScript;

    // Jump var (hidden)
    private float verticalVelocity;
    private float horizontalVelocity; // Not jump, but goes with Y-velocity
    private int jumpCount = 0; // How many jumps done before grounded
    
    private Collider[] colliders;


    // Attack var (hidden)
    private float attackTimer = 0f; // If 0f, the player can attack
                                    // again (no combos)
    private float currentPeriod = 0; // If <= 0, can check attackCollider
    private bool attackTimerActivated = false; // If true, activate he attack timer
    private ComboTemplate currentAttack;
    private int inputAttackIndex = 0;
    

    // Other movements var (public)
    private float waySign; // If 1: player looks to the right
    private bool wasGrounded;

    // Other movements var (private)
    private Vector3 moveVector;
    private CharacterController charaControl;

    private void Start()
    {
        // When lobby -> inGame
        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);

        charaControl = GetComponent<CharacterController>();
        InvulnerableTimer = 0f;
        moveVector = Vector3.zero;
    }

    public Vector3 GetMoveVector() { return moveVector; }

    [Command] public void CmdAddPosY(float y)
    {
        RpcAddPosY(y);
    }

    [ClientRpc] private void RpcAddPosY(float y)
    {
        if (hasAuthority)
            transform.localPosition = new Vector3(transform.localPosition.x,
                transform.localPosition.y + y, 0);
    }

    [Command] public void CmdSetVerticalVelocity(float vel)
    {
        RpcSetVerticalVelocity(vel);
    }

    [ClientRpc] private void RpcSetVerticalVelocity(float vel)
    {
        if (hasAuthority)
            verticalVelocity = vel;
    }

    [Command] public void CmdAddVelocities(float dvx, float dvy)
    {
        RpcChangeVelocities(dvx, dvy);
    }

    [ClientRpc] private void RpcChangeVelocities(float dvx, float dvy)
    {
        if (hasAuthority) {
            horizontalVelocity += dvx;
            verticalVelocity += dvy;
        }
    }

    public void Movements(float xInput, bool jumpButtonPressed, bool[] inputs)
    {
        /* Change the moveVector based on differents forces and inputs
         * See the functions called Movement_x to see the detail */

        // We update the timers
        UpdateTimers();

        if (hasAuthority) {
            CmdSyncPosXY(transform.localPosition);
            CmdSyncRotation(transform.localRotation);
        }
        else {
            transform.localPosition = Vector3.Lerp(transform.localPosition,
                syncPos, 0.25f);
            transform.localRotation = syncRotation;
        }

        // We reset moveVector and do things to velocities
        moveVector = Vector3.zero;

        if (isGrounded) {
            if (!wasGrounded) {
                verticalVelocity = Mathf.Max(verticalVelocity, -1f);
                horizontalVelocity = 0;
                wasGrounded = true;
            }
        } else wasGrounded = false;

        // Movement functions
        if (animScript == null || !hasAuthority) {
            // No animations
            Movement_Run(xInput);
            Movement_Jump(jumpButtonPressed);
            Movement_Attack(inputs);
        } else {
            // With animations
            animScript.CmdSyncIsRunning(Movement_Run(xInput));
            Movement_Jump(jumpButtonPressed);
            animScript.CmdSyncIsAttacking(Movement_Attack(inputs));
        }

        if (animScript != null) // Animations
            animScript.Do_animations(xInput, InvulnerableTimer);

        // Other useful functions
        CheckRotation(xInput);

        // Added velocities
        moveVector += new Vector3(horizontalVelocity, verticalVelocity, 0);

        // And finally move the player
        charaControl.Move(moveVector * Time.deltaTime);
        isGrounded = charaControl.isGrounded;
    }

    public bool LookToRight()
    {
        /* Returns true if the player is facing to the right */

        return Mathf.Abs(transform.localEulerAngles.y) <= 1f
            || transform.localEulerAngles.y >= 359f;
    }

    private void UpdateTimers()
    {
        if (transform.name == "Player Human" && Input.GetKeyDown(KeyCode.H)) {
            InvulnerableTimer = 0.5f;
        }

        // Attack Timer
        if (attackTimer > 0f) { attackTimer -= Time.deltaTime; }

        // Invulnerable Timer
        if (InvulnerableTimer > 0f) { InvulnerableTimer -= Time.deltaTime; }

        // Attack-Collision Timer
        if (attackTimerActivated) {
            currentPeriod = Mathf.Max(0f, currentPeriod - Time.deltaTime);

            if (currentPeriod <= 0f) {
                currentAttack.CollidersAttack();

                // We reset the attack timer
                currentPeriod = period;
            }
        }

        // Change color if hit (DEBUG)
        if (transform.name == "Player 2") {
            if (InvulnerableTimer > 0) {
                GetComponent<Renderer>().material.color = Color.gray;
            } else {
                GetComponent<Renderer>().material.color = Color.yellow;
            }
        }

        // Color of attack collider
        if (attackTimer > 0f) {
            // Here should be called an attack animation
            if (attackTimer < 0.5f) {
                // Just for seeing when we can combo (DEBUG)
                //currentAttack.attackCollider.gameObject.GetComponent<Renderer>()
                //.material.color = Color.yellow;
            } else {
                // Give a color to the collider (DEBUG)
                //currentAttack.attackCollider.gameObject.GetComponent<Renderer>()
                //.material.color = Color.black;
            }
        } else if (currentAttack != null) { // Attack just finished
                                            // Remove color of the collider (DEBUG)
                                            //currentAttack.attackCollider.gameObject.GetComponent<Renderer>()
                                            //    .material.color = Color.white;
        }
    }

    private bool Movement_Run(float xInput)
    {
        /* Make the player run based on xInput */
        if (Mathf.Abs(xInput) >= 0.25) {
            moveVector += new Vector3(xInput * horizontalSpeed, 0, 0);
            return true;
        } else { return false; }
    }

    private bool Movement_Jump(bool jumpButtonPressed)
    {
        /* Verify if a jump can be made, if so, makes the player jump
         * Returns a bool indicating if a jump has been made */

        if (charaControl.isGrounded) {
            jumpCount = 0;
        } else {
            if (verticalVelocity > 0 && CheckCollisionUp()) {
                verticalVelocity = 0;
            }

            // Gravity here
            verticalVelocity -= gravity * Time.deltaTime;

            // If we fall without making a jump, we lose 1 jump
            if (jumpCount == 0) { jumpCount = 1; }
        }

        if (jumpButtonPressed
          && jumpCount < jumpMax
          && verticalVelocity < 0.5f * jumpForce) {
            verticalVelocity = jumpForce;
            jumpCount++;
            return true;
        }

        return false;
    }

    private bool Movement_Attack(bool[] inputs)
    {
        /* Check if an attack can be made, if so, make the player attack
         * There is also combos (& combos limit)
         * Returns true if an attack has been made (else false) */

        if (attackTimer <= 0f) { // Attack finished || No attack

            // Here we should finish an attack animation

            if (currentAttack != null) { // Attack just finished
                currentAttack.actualCombo = -1; // We reset the combo
                currentAttack = null;
                inputAttackIndex = 0;
                attackTimerActivated = false; // And also the attacks timer
            }

            // We check if new attack
            for (int i = 0; i < listAttacks.Length; i++) {
                if (inputs[i]) {
                    currentAttack = listAttacks[i];
                    inputAttackIndex = i;
                }
            }
        }

        // Attacks & Combos
        if (currentAttack != null
          && inputs[inputAttackIndex]
          && attackTimer < 0.5f
          && currentAttack.actualCombo < currentAttack.comboLength - 1) {
            // We activate the attack timer
            attackTimerActivated = true;
            currentPeriod = period;

            currentAttack.actualCombo++;
            currentAttack.CollidersAttack();
            attackTimer = currentAttack.attackCooldown;
        }

        return attackTimer > 0f;
    }

    private void CheckRotation(float xInput)
    {
        /* Check if the player is correctly rotated
         * If not, rotate it correctly */

        waySign = LookToRight() ? 1 : -1;

        if (waySign * xInput < 0) {
            transform.Rotate(new Vector3(0, waySign * 180));
        }
    }

    private bool CheckCollisionUp()
    {
        colliders = Physics.OverlapBox(jumpCollider.bounds.center,
                                       jumpCollider.bounds.extents,
                                       jumpCollider.transform.rotation,
                                       LayerMask.GetMask("Plateform"));

        return colliders.Length > 0;
    }
}