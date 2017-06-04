using UnityEngine;

public class ShootAttack : AttackTemplate
{
    private void Start()
    {
        inputKey = "f";
        attackCooldown = 0.25f;
    }

    public override void Attack()
    {

    }
}
