using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AttackTemplate
{
    private void Start()
    {
        /* Set-up all the objets variables here */

        ComboLength = 4;

        powerRel = new float[] { 5, 5, 5, 20 };
        pushRel = new float[] { 0, 0, 0, 10 };

        dirVector = new Vector3[] {
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            new Vector3(0, 10, 0)
        };
    }
}
