using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackClass
{
    [Header("Attack vars")]
    [Space(5)]
    public float power;
    public string inputKey; // Input.GetKeyDown(inputKey)
    public float attackCooldown = 1; // The time to end the attack (s)
    public int comboIncrease = 1;

    [Space]

    [Header("Collider vars")]
    [Space(5)]
    public Collider attackCollider;

    public AttackClass(float _power,
            Collider _attackCollider,
            string _inputKey,
            float _attackCooldown = 1,
            int _comboIncrease = 1)
    {
        power = _power;
        attackCollider = _attackCollider;
        inputKey = _inputKey;
        attackCooldown = _attackCooldown;
        comboIncrease = _comboIncrease;
    }
}