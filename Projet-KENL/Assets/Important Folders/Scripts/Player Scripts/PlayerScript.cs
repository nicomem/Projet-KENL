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

    // TODO: give playerName to each in menuMulti
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

    #region SyncVar: syncPos
    [HideInInspector] [SyncVar] public Vector3 syncPos;
    public void SyncPos(Vector3 p)
    {
        if (isServer || !isNetworked) syncPos = p;
        else CmdSyncPos(p);
    }
    [Command] private void CmdSyncPos(Vector3 p) { syncPos = p; }
    #endregion

    #region SyncVar: syncRotation
    [HideInInspector] [SyncVar] public Quaternion syncRotation;
    public void SyncRotation(Quaternion r)
    {
        if (isServer || !isNetworked) syncRotation = r;
        else CmdSyncRotation(r);
    }
    [Command] private void CmdSyncRotation(Quaternion r) { syncRotation = r; }
    #endregion

    #region SyncVar: InvulnerableTimer
    // If <= 0f, player can get hit, resets in function of the attack
    [HideInInspector] [SyncVar] public float InvulnerableTimer;
    public void SyncInvulnerableTimer(float f)
    {
        if (isServer || !isNetworked) InvulnerableTimer = f;
        else CmdSyncInvulnerableTimer(f);
    }
    [Command] private void CmdSyncInvulnerableTimer(float t) { InvulnerableTimer = t; }
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

    #region SyncVar: verticalVelocity
    [HideInInspector] [SyncVar] public float verticalVelocity;
    public void SyncVerticalVelocity(float f)
    {
        if (isServer || !isNetworked) verticalVelocity = f;
        else CmdSyncVerticalVelocity(f);
    }
    [Command] private void CmdSyncVerticalVelocity(float f) { verticalVelocity = f; }
    #endregion

    #region SyncVar: horizontalVelocity
    [HideInInspector] [SyncVar] public float horizontalVelocity;
    public void SyncHorizontalVelocity(float f)
    {
        if (isServer || !isNetworked) horizontalVelocity = f;
        else CmdSyncHorizontalVelocity(f);
    }
    [Command] private void CmdSyncHorizontalVelocity(float f) { horizontalVelocity = f; }
    #endregion

    #region Others
    public void AddPosY(float y)
    {
        if (isServer || !isNetworked)
            syncPos = new Vector3(transform.localPosition.x,
                transform.localPosition.y + y, 0);
        else
            CmdAddPosY(y);
    }
    [Command]
    private void CmdAddPosY(float y)
    {
        transform.localPosition = new Vector3(transform.localPosition.x,
            transform.localPosition.y + y, 0);
    }

    public void AddVelocities(float dvx, float dvy)
    {
        if (isServer || !isNetworked) {
            horizontalVelocity += dvx;
            verticalVelocity += dvy;
        } else
            CmdChangeVelocities(dvx, dvy);
    }
    [Command]
    private void CmdChangeVelocities(float dvx, float dvy)
    {
        horizontalVelocity += dvx;
        verticalVelocity += dvy;
    }
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
    public ComboTemplate[] listAttacks;
    public int maxCombo = 4;
    public float period = 0.2f; // Set the period between each attackCollider check

    [Header("Others")]
    public AnimationsScript animScript;
    #endregion

    #region Private vars
    private int jumpCount = 0; // How many jumps done before grounded
    private Collider[] colliders;

    //Code for usual Health Bar
    public float MaxHealth = 100f;
    public float Health = 100f;
    public float percentOfHealth;
    public float hpBarLength;
    public Texture2D hpBarTexture;
    //End of usual healthbar

    private float attackTimer = 0f; // If 0f, the player can attack
                                    // again (no combos)
    private float currentPeriod = 0; // If <= 0, can check attackCollider
    private bool attackTimerActivated = false; // If true, activate he attack timer
    private ComboTemplate currentAttack;
    private int inputAttackIndex = 0;
    private bool isGrounded;

    private float waySign; // If 1: player looks to the right
    private bool isNetworked; // See CharaControlScript

    private Vector3 moveVector;
    private CharacterController charaControl;

    
    #endregion
    #endregion

    private void Start()
    {
        // When lobby -> inGame
        if (transform.parent == null)
            DontDestroyOnLoad(gameObject);

        charaControl = GetComponent<CharacterController>();
        InvulnerableTimer = 0f;
        moveVector = Vector3.zero;

        isNetworked = GameObject.Find("Network Manager") != null;
    }

    public void Movements(float xInput, bool jumpButtonPressed, int attackSelected)
    {
        /* Change the moveVector based on differents forces and inputs
         * See the functions called Movement_x to see the detail */

        // We update the timers
        UpdateTimers();

        if (isNetworked) {
            if (hasAuthority) {
                SyncPos(transform.position);
                SyncRotation(transform.rotation);
            } else {
                transform.position = Vector3.Lerp(transform.position,
                    syncPos, 0.33f);
                transform.rotation = syncRotation;
            }
        }

        // We reset moveVector and do things to velocities
        moveVector = Vector3.zero;

        horizontalVelocity /= (1 + 3 * Time.deltaTime);

        if (isGrounded) {
            verticalVelocity = Mathf.Max(-1f, verticalVelocity);
            horizontalVelocity *= 0.75f;
        }

        // Movement functions
        if (animScript == null || (!hasAuthority && isNetworked)) {
            // No animations
            Movement_Run(xInput);
            Movement_Jump(jumpButtonPressed);
            Movement_Attack(attackSelected);
        } else {
            animScript.SyncIsRunning(Movement_Run(xInput));
            Movement_Jump(jumpButtonPressed);
            animScript.SyncIsAttacking(Movement_Attack(attackSelected));
            animScript.SyncIsHit(InvulnerableTimer > 0);
        }

        if (animScript != null) // Animations
            animScript.Do_animations();

        // Other useful functions
        CheckRotation(xInput);

        // Added velocities
        moveVector += new Vector3(horizontalVelocity, verticalVelocity, 0);

        // And finally move the player
        charaControl.Move(moveVector * Time.deltaTime);
        isGrounded = charaControl.isGrounded;
    }

    private void UpdateTimers()
    {
        var deltaTime = Time.deltaTime;

        // Attack Timer
        if (attackTimer > 0f) attackTimer -= deltaTime;

        // Invulnerable Timer
        if (InvulnerableTimer > 0f) InvulnerableTimer -= deltaTime;

        // Attack-Collision Timer
        if (attackTimerActivated) currentPeriod -= deltaTime;
    }

    #region Movement functions
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
                if (hasAuthority) SyncVerticalVelocity(0);
                else verticalVelocity = 0;
            }

            // Gravity here
            verticalVelocity -= gravity * Time.deltaTime;
        }

        if (jumpButtonPressed
          && jumpCount < jumpMax
          && verticalVelocity < 0.5f * jumpForce) {
            SyncVerticalVelocity(jumpForce);
            SyncHorizontalVelocity(0.25f * horizontalVelocity);
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

        // When attack just finished
        if (attackTimer <= 0f && currentAttack != null) {
            currentAttack.actualCombo = -1; // We reset the combo
            currentAttack = null;
            inputAttackIndex = 0;
            attackTimerActivated = false; // And also the attacks timer
        }

        // Combo Timer
        if (attackTimerActivated && currentPeriod <= 0f) {
            currentAttack.CollidersAttack();

            // We reset the attack timer
            currentPeriod = period;
        }

        // Attacks & Combos
        if (attackSelected != -1 && CanAttack()) {
            currentAttack = listAttacks[attackSelected];

            // We activate the attack timer
            attackTimerActivated = true;
            currentPeriod = period;

            currentAttack.actualCombo++;
            currentAttack.CollidersAttack();
            attackTimer = currentAttack.attackCooldown;
        }

        return attackTimer > 0f;
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

        return Mathf.Abs(transform.localEulerAngles.y) <= 1f
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
        return CanStartAttack() || CanContinueCombo();
    }

    public bool CanStartAttack()
    {
        return attackTimer <= 0f;
    }

    public bool CanContinueCombo()
    {
        return currentAttack != null && attackTimer < 0.5f
          && currentAttack.actualCombo < currentAttack.comboLength - 1;
    }

    public bool CanBeHit()
    {
        return InvulnerableTimer <= 0.5f;
    }

    public bool IsHit()
    {
        return InvulnerableTimer > 0;
    }
    #endregion
}