using UnityEngine;

public class PickUpObjects : MonoBehaviour
{
    public float vitesseBonus;

    [HideInInspector] public int BonusWanted;
    private float savedTime;

    private CharacterController rb;
    private PlayerScript playerScript;

    private int OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Objet-Vitesse")) BonusWanted = 1;
        else if (other.gameObject.CompareTag("Objet-HP")) BonusWanted = 2;
        else if (other.gameObject.CompareTag("Objet-Attaque")) BonusWanted = 3;
        else return 0;

        other.gameObject.SetActive(false);
        savedTime = Time.time;

        return BonusWanted;
    }
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<CharacterController>();
        playerScript = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //float savedTime = null;
        if (BonusWanted == 1)
        {
            if (Time.time - savedTime <= 10)
            {
                float mouvement = Input.GetAxis("Horizontal");
                Vector2 move = new Vector2(mouvement * playerScript.horizontalSpeed, 0.0f);
                rb.Move(move * vitesseBonus * Time.deltaTime);
            }
            else
                vitesseBonus = 0;
        }
	}
}
