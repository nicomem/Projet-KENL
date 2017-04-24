using UnityEngine;

[System.Serializable]
public class ComboTemplate : MonoBehaviour
{
    /* Do not use this monoClass. Instead, create another monoClass that
     * inherit from this one (and sets its value) to create a combo */

    // Combo properties
    public Vector3 dirLastAttack;
    public float powerLastAttack;
    public float multPushLastAttack = 1; // For throwing farther with the same power
    public int comboLength;

    public string inputKey;
    public Collider attackCollider;
    public float attackCooldown;

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

                Debug.Log(playerHit.transform.name);

                leftRight = GetComponent<PlayerScript>()
                    .LookToRight() ? 1f : -1f;

                // If player can get hit
                if (playerHit.CanBeHit()) {
                    if (actualCombo < comboLength - 1) {
                        GiveAttack(dirLastAttack, powerLastAttack / 2,
                            leftRight, multPushLastAttack / 5);
                    } else {
                        GiveAttack(dirLastAttack, powerLastAttack,
                            leftRight, multPushLastAttack);
                    }
                }
            }
        }
    }

    protected void GiveAttack(Vector3 attackDir, float attackPower,
        float leftRight, float multPush)
    {
        // TODO: Verify health system
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
