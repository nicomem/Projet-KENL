using UnityEngine;
using System.Collections.Generic;
using System;

public class PickUpObjects : MonoBehaviour
{
    public float vitesseBonus;
    public float Inversion;

    private float[] savedTime; // Time since object picked (default: -100f)

    private CharacterController rb;
    private PlayerScript playerScript;

    private List<string> bonusString;
    private Action[] ActivationFunc;

    void Start()
    {
        rb = GetComponent<CharacterController>();
        playerScript = GetComponent<PlayerScript>();

        bonusString = new List<string> { "Objet-Vitesse", "Objet-HP",
            "Objet-Attaque", "Objet-Invert", "Objet-VitesseMalus" };

        savedTime = new float[bonusString.Count];
        for (int i = 0; i < savedTime.Length; i++)
            savedTime[i] = -100f;

        ActivationFunc = new Action[] {
            // Objet-Vitesse
            () => { playerScript.horizontalSpeed *= 2f; },

            // Objet-HP
            () => {
                playerScript.percentHealth = 
                    Mathf.Min(0, playerScript.percentHealth - 50);
                savedTime[bonusString.FindIndex(s => s == "Objet-HP")] = -100f;
            },

            // Objet-Attaque
            () => { playerScript.bonusAttack = 2f; },

            // Objet-Invert
            () => { playerScript.horizontalSpeed *= -1f; },

            // Objet-VitesseMalus
            () => { playerScript.horizontalSpeed *= 0.5f; }
        };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerScript.bonusAttack = 1f;
        vitesseBonus = 0;
        playerScript.horizontalSpeed = 20f;

        for (int i = 0; i < savedTime.Length; i++) {
            if (Time.time - savedTime[i] <= 10)
                ActivationFunc[i]();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Object"))
            return;

        Destroy(other.gameObject);

        int index = bonusString.FindIndex(s => s == other.gameObject.tag);
        savedTime[index] = Time.time;
    }

    public void ResetBonuses()
    {
        for (int i = 0; i < savedTime.Length; i++)
            savedTime[i] = -100f;
    }
}