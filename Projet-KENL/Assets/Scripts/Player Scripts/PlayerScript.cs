using UnityEngine;

[System.Serializable]
public class PlayerScript : MonoBehaviour
{
    // Jump var (Editor)
    [Header("Jumping")]
    [Space(5)]
    public float gravity = 14.0f;
    public float jumpForce = 10.0f; // Y-velocity added when jumping
    public float horizontalSpeed = 20.0f;
    public int jumpMax = 2; // How many jumps the player can do
                            // before being grounded

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
    [System.NonSerialized]
    public bool isGrounded = true; // See if grounded (can be modified if
                                   // needed, ex: attacks)
    private bool isJumping = false;

    // Attack var (hidden)
    public float percentHealth = 0; // pushReceived = 
                                    // power * (1 + percentHealth / 100)
    private float attackTimer = 0f; // If 0f, the player can attack
                                    // again (no combos)
    private bool isAttacking = false; // See if the player is attacking
    private float currentPeriod = 0; // If <= 0, can check attackCollider
    private bool attackTimerActivated = false; // If true, activate he attack timer
    private ComboTemplate currentAttack;
    private int inputAttackIndex = 0;

    public float InvulnerableTimer { get; set; }
    // If <= 0f, player can get hit, resets in function of the attack

    // Other movements var (public)
    private float waySign; // If 1: player looks to the right
    private bool isRunning = false;

    // Other movements var (private)
    private Vector3 moveVector;
    private CharacterController charaControl;

    private void Start()
    {
        charaControl = GetComponent<CharacterController>();
        InvulnerableTimer = 0f;
        moveVector = Vector3.zero;
    }

    public Vector3 GetMoveVector() { return moveVector; }
    public void SetHorizontalVelocity(float vel) { horizontalVelocity = vel; }
    public void SetVerticalVelocity(float vel) { verticalVelocity = vel; }

    public void Movements(float xInput, bool jumpButtonPressed, bool[] inputs)
    {
        /* Change the moveVector based on differents forces and inputs
         * See the functions called Movement_x to see the detail */

        // We update the timers
        UpdateTimers();

        // We reset moveVector and do things to velocities
        moveVector = Vector3.zero;

        if (isGrounded) {
            verticalVelocity = Mathf.Max(verticalVelocity, -1f);
            horizontalVelocity = 0;
        }

        // Movement functions
        if (transform.name == "Player 2")
        {
            Movement_Run(xInput);
            Movement_Jump(jumpButtonPressed);
            Movement_Attack(inputs);
        }
        else
        {
            animScript.isRunning = Movement_Run(xInput);
            Movement_Jump(jumpButtonPressed);
            animScript.isAttacking = Movement_Attack(inputs);

            // Animations
            animScript.do_animations(xInput, InvulnerableTimer);
        }

        // Other useful functions
        CheckRotation(-xInput);

        // Added velocities
        AddMovement(new Vector3(horizontalVelocity, verticalVelocity, 0));

        // And finally move the player
        charaControl.Move(moveVector * Time.deltaTime);
        isGrounded = charaControl.isGrounded;
    }

    public void AddMovement(Vector3 movement)
    {
        /* Call this function to add a movement to this player */

        moveVector += movement;
    }

    public void ChangeVelocities(float dvx, float dvy)
    {
        /* Use that function if you want to change the X and/or Y velocities 
         * ex: gravity, attack or any other force */

        horizontalVelocity += dvx;
        verticalVelocity += dvy;
    }

    public bool IsCorrectWay()
    {
        /* Returns true if the player is facing to the right */
        return transform.eulerAngles.y <= 1f || transform.eulerAngles.y >= 179f;
    }


    private void UpdateTimers()
    {
        if (transform.name == "Player Human" && Input.GetKeyDown(KeyCode.H))
        {
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
        if (transform.name == "Player 2")
        {
            if (InvulnerableTimer > 0)
            {
                GetComponent<Renderer>().material.color = Color.gray;
            }
            else
            {
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
            AddMovement(new Vector3(xInput * horizontalSpeed, 0, 0));
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

        waySign = IsCorrectWay() ? 1 : -1;

        if (waySign * xInput < 0) {
            transform.Rotate(new Vector3(0, waySign * 180));
        }
    }
}