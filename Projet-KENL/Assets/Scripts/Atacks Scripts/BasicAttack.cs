using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : AttackTemplate
{
    private void Start()
    {
        /* Set-up all the objets variables here
         * For testing, you can use the Unity Editor
         * (If you set values here, changes in editor will not work at start) */

        ComboLength = 4;

        powerRel = new float[] { 5, 5, 5, 20 };
        pushRel = new float[] { 1, 1, 1, 10 };

        dirVector = new Vector3[] { // Normalized
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            new Vector3(0, 1, 0)
        };
        powerVector = new float[] { 0, 0, 0, 0 };
        pushVector = new float[] { 0, 0, 0, 5 };
    }
}
