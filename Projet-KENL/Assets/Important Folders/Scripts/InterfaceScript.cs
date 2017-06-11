using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceScript : MonoBehaviour {

    PlayerScript[] liste;
    public Text player1;
    public Text player2;
    public Text player3;
    public Text player4;
	// Use this for initialization
	void Start () {

        liste = GameObject.Find("Map Infos").GetComponent<MapInfosScript>().ListPlayerScripts;
        if (liste.GetLength(0) == 3)
            player4.enabled = false;
        if (liste.GetLength(0) == 2)
        {
            player3.enabled = false;
            player4.enabled = false;
        }
            

	}
	
	// Update is called once per frame
	void Update () {
        player1.text = liste[0].playerName + " " + liste[0].percentHealth ;
        player2.text = liste[1].playerName + " " + liste[1].percentHealth ;
        player3.text = liste[2].playerName + " " + liste[2].percentHealth ;
        player4.text = liste[3].playerName + " " + liste[3].percentHealth ;
    }
}
