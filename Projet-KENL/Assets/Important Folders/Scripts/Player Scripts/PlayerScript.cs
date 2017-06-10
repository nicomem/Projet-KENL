using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerScript : NetworkBehaviour
{
    #region Variables
    #region SyncVar region
    #region SyncVar: persoName
    [HideInInspector] [SyncVar] public string persoName;
    public void SyncPersoName(string s)
    {
        if (isServer || !isNetworked) persoName = s;
        else CmdSyncPersoName(s);
    }
    [Command] private void CmdSyncPersoName(string s) { persoName = s; }
    #endregion

    #region SyncVar: playerName
    [HideInInspector] [SyncVar] public string playerName;
    public void SyncPlayerName(string s)
    {
        if (isServer || !isNetworked) playerName = s;
        else CmdSyncPlayerName(s);
    }
    [Command] private void CmdSyncPlayerName(string s) { playerName = s; }
    #endregion

    #region SyncVar: isIA
    [HideInInspector] [SyncVar] public bool isIA;
    public void SyncIsIA(bool b)
    {
        if (isServer || !isNetworked) isIA = b;
        else CmdSyncIsIA(b);
    }
    [Command] private void CmdSyncIsIA(bool b) { isIA = b; }
    #endregion

    #region SyncVar: percentHealth
    [HideInInspector] [SyncVar] public float percentHealth = 0;
    public void SyncPercentHealth(float f)
    {
        if (isServer || !isNetworked) percentHealth = f;
        else CmdSyncPercentHealth(f);
    }
    [Command] private void CmdSyncPercentHealth(float h) { percentHealth = h; }
    #endregion

    #region SyncVar: persoLives
    [HideInInspector] [SyncVar] public float persoLives = 3;
    public void SyncPersoLives(float f)
    {
        if (isServer || !isNetworked) persoLives = f;
        else CmdSyncPersoLives(f);
    }
    [Command] private void CmdSyncPersoLives(float l) { persoLives = l; }
    #endregion

    #region SyncVar: isKO
    [HideInInspector] [SyncVar] public bool isKO = false;
    public void SyncIsKO(bool b)
    {
        if (isServer || !isNetworked) isKO = b;
        else CmdSyncIsKO(b);
    }
    [Command] private void CmdSyncIsKO(bool b) { isKO = b; }
    #endregion
    #endregion

    #region Editor modified
    [Header("Jumping")]
    public float gravity = 60f;
    public float jumpForce = 30f; // Y-velocity added when jumping
    public float horizontalSpeed = 20f;
    public int jumpMax = 2; // How many jumps the player can do
                            // before being grounded
    public Collider jumpCollider;

    [Header("Attacking")]
    public AttackTemplate[] listAttacks;
    public float period = 0.2f; // Set the period between each attackCollider check

    [Header("Others")]
    public AnimationsScript animScript;
    #endregion

    #region Other vars
    private int jumpCount = 0; // How many jumps done before grounded
    private Collider[] colliders;

    //Code for usual Health Bar
    //public float MaxHealth = 100f;
    //public float Health = 100f;
    //public float percentOfHealth;
    public float hpBarLength;
    public Texture2D hpBarTexture;
    //End of usual healthbar

    private float attackTimer = 0f; // If 0f, the player can attack
                                    // again (no combos)
    private float currentPeriod = 0; // If <= 0, can check attackCollider
    private bool attackTimerActivated = false; // If true, activate he attack timer
    private bool isGrounded;
    private float horizontalVelocity = 0f, verticalVelocity = 0f;
    private float invulnerableTimer;

    private float waySign; // If 1: player looks to the right
    private bool isNetworked; // See CharaControlScript
    
    private CharacterController charaControl;

    [HideInInspector] public BlockScript blockScript;
    public float bonusAttack = 1f;

    public bool IsBlocking { get; private set; }
    #endregion
    #endregion

    private void Start()
    {
        // When lobby -> inGame
        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);

        charaControl = GetComponent<CharacterController>();
        invulnerableTimer = 0f;

        isNetworked = GameObject.Find("Network Manager") != null;
        blockScript = GetComponent<BlockScript>();
    }

    public void SetVerticalVelocity(float vel)
    {
        verticalVelocity = vel;
    }

    public void SetHorizontalVelocity(float vel)
    {
        horizontalVelocity = vel;
    }

    public void GetHit(float x, float y, float attackCooldown, float attackPower)
    {
        if (isGrounded)
            charaControl.Move(new Vector3(0, 1, 0));

        horizontalVelocity += x;
        verticalVelocity += y;

        invulnerableTimer = attackCooldown;

        percentHealth += attackPower;
    }

    [Command]
    public void CmdMovements(float xInput, bool jumpButtonPressed,
        int attackSelected, bool blockPressed)
    {
        /* Change the moveVector based on differents forces and inputs
         * See the functions called Movement_x to see the detail */

        // We update the timers
        UpdateTimers();

        // We reset moveVector and do things to velocities
        Vector3 moveVector = Vector3.zero;

        horizontalVelocity /= (1 + 3 * Time.deltaTime);

        if (isGrounded) {
            verticalVelocity = Mathf.Max(-1f, verticalVelocity);
            horizontalVelocity *= 0.75f;
        }

        // Block is "blocking" other actions
        if (!isGrounded)
            blockPressed = false;

        if (blockPressed) {
            xInput = 0;
            jumpButtonPressed = false;
            attackSelected = -1;
        }

        bool isRunning, isAttacking, isHit;

        // Movement functions
        isRunning = Movement_Run(ref moveVector, xInput);
        Movement_Jump(jumpButtonPressed);
        Movement_Attack(attackSelected);
        Movement_Block(blockPressed);

        isAttacking = attackTimerActivated;
        isHit = invulnerableTimer > 0;
        IsBlocking = blockPressed;
        
        // Make the player rotate the right way
        CheckRotation(xInput);

        // Add velocities
        moveVector += new Vector3(horizontalVelocity, verticalVelocity, 0);

        // And finally move the player
        charaControl.Move(moveVector * Time.deltaTime);
        isGrounded = charaControl.isGrounded;

        RpcMovements(transform.position, transform.rotation,
            isRunning, isAttacking, isHit, IsBlocking);
    }

    [ClientRpc]
    private void RpcMovements(Vector3 position, Quaternion rotation,
        bool isRunning, bool isAttacking, bool isHit, bool isBlocking)
    {
        transform.position = position;
        transform.rotation = rotation;

        animScript.isRunning = isRunning;
        animScript.isAttacking = isAttacking;
        animScript.isHit = isHit;
        animScript.isBlocking = isBlocking;
    }

    private void UpdateTimers()
    {
        var deltaTime = Time.deltaTime;

        // Attack Timer
        if (attackTimer > 0f) attackTimer -= deltaTime;

        // Invulnerable Timer
        if (invulnerableTimer > 0f) invulnerableTimer -= deltaTime;

        // Attack-Collision Timer
        if (attackTimerActivated) currentPeriod -= deltaTime;
    }

    #region Movement functions
    private bool Movement_Run(ref Vector3 moveVector, float xInput)
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
                if (hasAuthority) verticalVelocity = 0;
                else verticalVelocity = 0;
            }

            // Gravity here
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if (jumpButtonPressed
          && jumpCount < jumpMax
          && verticalVelocity < 0.5f * jumpForce) {
            verticalVelocity = jumpForce;
            horizontalVelocity = 0.25f * horizontalVelocity;
            jumpCount++;
            return true;
        }

        return false;
    }

    private bool Movement_Attack(int attackSelected)
    {
        /* Check if an attack can be made, if so, make the player attack
         * There is also combos (& combos limit)
         * Returns true if an attack is being made (else false) */

        // When attack finished => indicate we're not attacking
        if (CanAttack())
            attackTimerActivated = false;

        // Attacks
        if (attackSelected != -1 && CanAttack()) {
            // We activate the attack timer
            attackTimerActivated = true;
            currentPeriod = period;

            AttackTemplate attack = listAttacks[attackSelected];
            attack.Attack();
            attackTimer = attack.attackCooldown;

            return true;
        }

        return false;
    }

    private bool Movement_Block(bool blockPressed)
    {
        if (blockPressed) {
            blockScript.Block();
            return true;
        } else {
            blockScript.StopBlocking();
            return false;
        }
    }
    #endregion

    #region Check functions
    private void CheckRotation(float xInput)
    {
        /* Check if the player is correctly rotated
         * If not, rotate it correctly */

        waySign = LookToRight() ? 1 : -1;

        if (waySign * xInput < 0) {
            transform.Rotate(new Vector3(0, waySign * 180));
        }
    }

    public bool LookToRight()
    {
        /* Returns true if the player is facing to the right */

        return transform.localEulerAngles.y <= 1f
            || transform.localEulerAngles.y >= 359f;
    }

    private bool CheckCollisionUp()
    {
        colliders = Physics.OverlapBox(jumpCollider.bounds.center,
                                       jumpCollider.bounds.extents,
                                       jumpCollider.transform.rotation,
                                       LayerMask.GetMask("Plateform"));

        return colliders.Length > 0;
    }

    public bool CanAttack()
    {
        return attackTimer <= 0f;
    }

    public bool CanBeHit()
    {
        return invulnerableTimer <= 0.5f;
    }

    public bool IsHit()
    {
        return invulnerableTimer > 0;
    }
    #endregion
}