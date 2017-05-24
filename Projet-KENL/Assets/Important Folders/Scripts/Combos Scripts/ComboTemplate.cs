using UnityEngine;

[System.Serializable]
public class ComboTemplate : MonoBehaviour
{
    /* Do not use this monoClass. Instead, create another monoClass that
     * inherit from this one (and sets its value) to create a combo */

    // Combo properties
    public Vector3 dirLastAttack = new Vector3(0.8f, 0.2f, 0);
    public float powerLastAttack = 11;
    public float multPushLastAttack = 5; // For throwing farther with the same power
    public int comboLength = 1;

    public string inputKey = "e";
    public Collider attackCollider;
    public float attackCooldown = 0.5f;

    // Other variables
    [System.NonSerialized]
    public int actualCombo = -1; // We increment before the attack
                                 // so at first attack will be at 0
    protected Collider[] colliders;
    protected PlayerScript playerHit; // Player script of hit player
    protected float leftRight; // 1f if attack to the right, -1f else
    protected float healthMultiplier;
    protected float x, y;

    protected bool isNetworked;

    private void Start()
    {
        isNetworked = GameObject.Find("Network Manager") != null;
    }

    public void CollidersAttack()
    {
        /* Checks the collisions of the attack (== attack active) */

        colliders = Physics.OverlapBox(attackCollider.bounds.center,
                                       attackCollider.bounds.extents,
                                       attackCollider.transform.rotation,
                                       LayerMask.GetMask("Hitbox"));

        for (int i = 0; i < colliders.Length; i++) {
            playerHit = colliders[i].transform.parent
                .GetComponent<PlayerScript>();

            // No self-hitting
            if (playerHit.transform.GetInstanceID() != transform.GetInstanceID()) {

                leftRight = GetComponent<PlayerScript>()
                    .LookToRight() ? 1f : -1f;

                // If player can get hit
                if (playerHit.CanBeHit()) {
                    if (playerHit.isBlocking) {
                        powerLastAttack /= 2;
                        multPushLastAttack /= 2;
                    }

                    if (actualCombo < comboLength - 1) {
                        powerLastAttack /= 2;
                        multPushLastAttack /= 5;
                    }

                    GiveAttack(dirLastAttack, powerLastAttack,
                        leftRight, multPushLastAttack);
                }
            }
        }
    }

    protected void GiveAttack(Vector3 attackDir, float attackPower,
        float leftRight, float multPush)
    {
        healthMultiplier = 1 + (playerHit.percentHealth / 100);

        x = attackDir.x * attackPower * leftRight * healthMultiplier
            * multPush;
        y = attackDir.y * attackPower * healthMultiplier * multPush;

        // Place it in mid-air (== not grounded)
        playerHit.AddPosY(0.1f);
        playerHit.AddVelocities(x, y);
        playerHit.SyncInvulnerableTimer(attackCooldown);
        playerHit.SyncPercentHealth(playerHit.percentHealth + attackPower);
    }
}
