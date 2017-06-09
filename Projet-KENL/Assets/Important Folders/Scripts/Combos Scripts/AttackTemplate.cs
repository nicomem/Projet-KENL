using UnityEngine.Networking;

public abstract class AttackTemplate : NetworkBehaviour
{
    public string inputKey;
    public float attackCooldown;
    protected PlayerScript playerScript;

    public abstract void Attack();
}
