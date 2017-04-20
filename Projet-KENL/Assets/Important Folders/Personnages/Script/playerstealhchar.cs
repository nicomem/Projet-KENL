using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerstealhchar : MonoBehaviour {

    public Animator anim;
  

    private float inputX;
    private bool run; 

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
       
        run = false; 
            
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.Play("attack05", -1);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            anim.Play("gethit01", -1);
        }
        inputX = Input.GetAxis("Horizontal");
        anim.SetFloat("imputX", inputX);
        anim.SetBool("run", run);

   
        if (run && inputX > 0)
        {
            anim.Play("run00", -1);
        }


    }
}
