using UnityEngine;
using System.Collections.Generic;

public class PickUpObjects : MonoBehaviour
{
    public float vitesseBonus;

    public float Inversion;

    public bool bonus;

    [HideInInspector] public int BonusWanted;
    private float savedTime;

    private CharacterController rb;
    private PlayerScript playerScript;

    private List<string> bonusString;
    
    void Start ()
    {
        rb = GetComponent<CharacterController>();
        playerScript = GetComponent<PlayerScript>();

        bonusString = new List<string> { "", "Objet-Vitesse", "Objet-HP",
            "Objet-Attaque", "Objet-Invert", "Objet-VitesseMalus" };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerScript.bonusAttack = 1f;
        vitesseBonus = 0;
        playerScript.horizontalSpeed = 20f;

        switch (BonusWanted) {
            case 1:
                if (Time.time - savedTime <= 10) {
                    float mouvement = Input.GetAxis("Horizontal");
                    Vector2 move = new Vector2(mouvement * playerScript.horizontalSpeed, 0.0f);
                    rb.Move(move * vitesseBonus * Time.deltaTime);
                }
                break;

            case 2:
                playerScript.percentHealth = playerScript.percentHealth - 50;
                if (playerScript.percentHealth <= 0)
                    playerScript.percentHealth = 0;
                break;

            case 3:
                if (Time.time - savedTime <= 10)
                    playerScript.bonusAttack = 2f;
                break;

            case 4:
                if (Time.time - savedTime <= 10) {
                    float mouvement = Input.GetAxis("Horizontal");
                    Vector2 move = new Vector2(mouvement * playerScript.horizontalSpeed, 0.0f);
                    rb.Move(move * Inversion * Time.deltaTime);
                }
                break;

            case 5:
                if (Time.time - savedTime <= 10) {
                    playerScript.horizontalSpeed = 10f;
                }
                break;

            default:
                break;
        }
    }

    private int OnTriggerEnter(Collider other)
    {
        other.gameObject.SetActive(false);
        savedTime = Time.time;
        BonusWanted = bonusString.FindIndex(s => s == other.gameObject.tag);

        return BonusWanted;
    }
}
