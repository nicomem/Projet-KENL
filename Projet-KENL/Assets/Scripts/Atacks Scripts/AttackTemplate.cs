using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackTemplate : MonoBehaviour
{
    /* Do not use this monoClass. Instead, create another monoClass that
     * inherit from this one (and sets its value) to create an attack/combo */

    // The power (pv) and push (movement)
    // each attack from the combo gives to the other player

    // (direction based on the position of the two players)
    // (attackedPlayer.position - attacker.position).normalize
    public float[] powerRel;
    public float[] pushRel;

    // (direction given by the Vector3, magnitude of vector given by pushVector)
    // (power given by powerVector)
    public Vector3[] dirVector;
    public float[] pushVector;
    public float[] powerVector;

    // Other attack variables

    protected PlayerScript playerHit; // Player script of hit player

    public string InputKey;
    public float AttackCooldown;
    // \-> The time to end the attack (s)
    public int ComboIncrease;
    public int ComboLength;
    public Collider AttackCollider;


    public void CollidersAttack()
    {
        /* Checks the collisions of the attack (== attack active) */

        Collider[] colliders =
            Physics.OverlapBox(AttackCollider.bounds.center,
                               AttackCollider.bounds.extents,
                               AttackCollider.transform.rotation,
                               LayerMask.GetMask("Hitbox"));

        // Colliders gives 2 references to 2 Hitbox-Colliders
        // So we only take one half
        for (int i = 0; i < colliders.Length / 2; i++) {
            // No self-hitting here !
            if (colliders[i].transform.name != transform.name) {
                playerHit = colliders[i].GetComponent<PlayerScript>();

                // If player can get hit
                if (playerHit.InvulnerableTimer <= 0f) {
                    print(colliders[i].transform.name);
                }
            }
        }
    }
}