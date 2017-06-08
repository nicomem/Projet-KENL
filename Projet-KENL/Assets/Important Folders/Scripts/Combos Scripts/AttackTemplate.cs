using UnityEngine.Networking;

public abstract class AttackTemplate : NetworkBehaviour
{
    public string inputKey;
    public float attackCooldown;

    public abstract void Attack();
}
