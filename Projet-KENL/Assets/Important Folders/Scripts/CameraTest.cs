using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTest : MonoBehaviour {

    public GameObject Joueur;

    private Vector3 vue;

	// Use this for initialization
	void Start () {
        vue = transform.position - Joueur.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = Joueur.transform.position + vue;
	}
}
