using UnityEngine;

[System.Serializable]
public abstract class AttackTemplate : MonoBehaviour {
    public string inputKey;
    public float attackCooldown;

    public abstract void Attack();
}
