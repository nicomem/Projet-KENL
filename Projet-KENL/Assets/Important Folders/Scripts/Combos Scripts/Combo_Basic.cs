using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Combo_Basic : AttackTemplate
{
    public Vector3 dir = new Vector3(0.8f, 0.2f, 0);
    public float power = 5f;
    public float multPush = 10f; // For throwing farther with the same power

    public Collider attackCollider;
    private bool isNetworked;

    private void Start()
    {
        isNetworked = GameObject.Find("Network Manager") != null;

        inputKey = "e";
        attackCooldown = 0.5f;
    }

    public override void Attack()
    {
        if (isServer || !isNetworked)
            BeginAttack();
        else
            CmdBeginAttack();
    }

    [Command]
    private void CmdBeginAttack()
    {
        BeginAttack();
    }

    private void BeginAttack()
    {
        /* Checks the collisions of the attack (== attack active) */

        Collider[] colliders = Physics.OverlapBox(attackCollider.bounds.center,
            attackCollider.bounds.extents,
            attackCollider.transform.rotation,
            LayerMask.GetMask("Hitbox"));

        PlayerScript playerHit;
        for (int i = 0; i < colliders.Length; i++) {
            playerHit = colliders[i].transform.parent
                .GetComponent<PlayerScript>();

            // No self-hitting
            if (playerHit.transform.GetInstanceID() != transform.GetInstanceID()) {

                float leftRight = GetComponent<PlayerScript>()
                    .LookToRight() ? 1f : -1f;

                // If player can get hit
                if (playerHit.CanBeHit()) {
                    if (playerHit.isBlocking) {
                        power /= 2;
                        multPush /= 2;
                    }

                    GiveAttack(playerHit, dir, power * playerScript.bonusAttack,
                        leftRight, multPush);
                }
            }
        }
    }

    private void GiveAttack(PlayerScript playerHit, Vector3 attackDir,
        float attackPower, float leftRight, float multPush)
    {
        float healthMultiplier = 1 + (playerHit.percentHealth / 100);

        float x = attackDir.x * attackPower * leftRight * healthMultiplier
            * multPush;
        float y = attackDir.y * attackPower * healthMultiplier * multPush;

        // Place it in mid-air (== not grounded)
        playerHit.AddPosY(0.1f);
        playerHit.AddVelocities(x, y);
        playerHit.SyncInvulnerableTimer(attackCooldown);
        playerHit.SyncPercentHealth(playerHit.percentHealth + attackPower);
    }
}
