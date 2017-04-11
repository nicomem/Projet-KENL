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
    protected float attackDirection; // 1f if attack to the right, -1f else
    protected float healthMultiplier;
    protected float x, y;

    public void CollidersAttack()
    {
        /* Checks the collisions of the attack (== attack active) */

        colliders = Physics.OverlapBox(attackCollider.bounds.center,
                                       attackCollider.bounds.extents,
                                       attackCollider.transform.rotation,
                                       LayerMask.GetMask("Hitbox"));

        // Colliders gives 2 references to 2 Hitbox-Colliders
        // So we only take one half
        for (int i = 0; i < colliders.Length / 2; i++) {
            // No self-hitting here !
            if (colliders[i].transform.name != transform.name) {
                playerHit = colliders[i].GetComponent<PlayerScript>();

                attackDirection = GetComponent<PlayerScript>()
                    .LookToRight() ? 1f : -1f;

                // If player can get hit
                if (playerHit.InvulnerableTimer <= 0.5f) {
                    if (actualCombo < comboLength - 1) {
                        GiveAttack(Vector3.zero, 0f, 0f, 0f);
                    } else {
                        GiveAttack(dirLastAttack, powerLastAttack,
                            attackDirection, multPushLastAttack);
                    }
                }
            }
        }
    }

    protected void GiveAttack(Vector3 attackDir, float attackPower,
        float attackDirection, float multPush)
    {
        // TODO: Verify health system
        healthMultiplier = (1 + (playerHit.percentHealth / 100));

        x = attackDir.x * attackPower * attackDirection * healthMultiplier
            * multPush;
        y = attackDir.y * attackPower * healthMultiplier * multPush;

        playerHit.CmdChangeVelocities(x, y);
        playerHit.percentHealth += attackPower;
        playerHit.InvulnerableTimer = attackCooldown;
        playerHit.isGrounded = false;
    }
}
