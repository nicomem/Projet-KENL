using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawnObject : MonoBehaviour {

    public GameObject ObjetAttaque;
    public GameObject ObjetInvert;
    public GameObject ObjetPV;
    public GameObject ObjetVitesse;
    public GameObject ObjetVitesseMalus;

    private float time;
    private float spawnTime = 5f;

    private int test = 0;
    public Vector3 positionToSpawn;

    System.Random rand = new System.Random();
	// Use this for initialization
	void Start ()
    {
        positionToSpawn = new Vector3(0, 8, 0); 
        whatPrefab();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {   
        //if (time >= spawnTime)
        //compte
        time += Time.deltaTime;

        //Check si cst le bon moment de spawn les objets
        if (time >= spawnTime)
        {
            time = 0;
            if (test == 1)
                Instantiate(ObjetAttaque, positionToSpawn, Quaternion.identity);
            else if (test == 2)
                Instantiate(ObjetPV, positionToSpawn, Quaternion.identity);
            else if (test == 3)
                Instantiate(ObjetInvert, positionToSpawn, Quaternion.identity);
            else if (test == 4)
                Instantiate(ObjetVitesse, positionToSpawn, Quaternion.identity);
            else if (test == 5)
                Instantiate(ObjetVitesseMalus, positionToSpawn, Quaternion.identity);
            whatPrefab();
        }
    }

    void whatPrefab()
    {
        test = rand.Next(1, 6);
    }
}
