using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackClass
{
    public float power;
    public Collider attackCollider;
    public string inputKey; // Input.GetKeyDown(inputKey)
    public float attackCooldown = 1; // The time to end the attack (s)
    public int comboIncrease = 1;

    public AttackClass(float _power,
            Collider _attackCollider,
            string _inputKey,
            float _attackCooldown,
            int _comboIncrease)
    {
        power = _power;
        attackCollider = _attackCollider;
        inputKey = _inputKey;
        attackCooldown = _attackCooldown;
        comboIncrease = _comboIncrease;
    }
}